using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Centvrio.Emoji;
using LeChuck.ReferralLinks.DataAccess.Entities;
using LeChuck.ReferralLinks.DataAccess.Repositories;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Lambda.Timer.Processors
{

    public interface ISweepProcessor
    {
        Task Sweep();
    }

    public class SweepProcessor : ISweepProcessor
    {
        private readonly ILogger<SweepProcessor> _logger;
        private readonly ITimedTasksRepository _repository;
        private readonly IBotService _bot;
        private readonly AppConfiguration _config;

        public SweepProcessor(ILogger<SweepProcessor> logger, ITimedTasksRepository repository, IBotService bot, AppConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task Sweep()
        {
            var sweepDatetime = DateTime.Now;
            _logger.LogInformation($"Initiating sweep at {sweepDatetime:u}");
            
            var pending = (await _repository.GetPendingTasks(sweepDatetime)).ToList();
            _logger.LogInformation($"Processing {pending.Count()} messages ...");
            
            var tasks = pending.SelectMany(ProcessMessage).ToList();
            await Task.WhenAll(tasks);
            
            _logger.LogInformation($"Done! ... {tasks.Count()} messages sent.");
        }

        async Task SendMessage(Channel channel, LinkData data)
        {
            try
            {
                _logger.LogInformation($"Sending Link {data} to ");
                var message = new StringBuilder();
                message.Append($"\n{Event.Ribbon} <b>{data.Title}</b>\n\n");
                if (!string.IsNullOrWhiteSpace(data.OriginalPrice))
                    message.Append($"{OtherSymbols.CrossMark} PVP: {data.OriginalPrice}\n");
                message.Append($"{Money.Euro} <b>PRECIO FINAL: {data.FinalPrice}</b>\n");
                if (!string.IsNullOrWhiteSpace(data.SavedPrice))
                    message.Append($"{Clothing.Purse} Ahorras {data.SavedPrice}\n");
                message.Append($"\n{HouseHold.ShoppingCart} {data.ShortenedUrl}");

                await _bot.SendPhotoAsync(channel.ChannelId, data.PictureUrl, message.ToString(), TextModeEnum.Html);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot send message!:\n" +
                                 $"  Channel:{channel}\n" +
                                 $"  Data:{JsonSerializer.Serialize(data)}\n" +
                                 $"  Exception: {ex.Message}\n{ex.StackTrace}");
            }
        }

        List<Task> ProcessMessage(TimedTaskDbEntity data)
        {
            var tasks = data.Message.Channels.Select(channel => SendMessage(channel, data.Message)).ToList();
            data.NextRun = DateTime.Now.AddMinutes(data.RunSpan.Minutes);
            tasks.Add(_repository.SaveItemAsync(data));
            return tasks;
        }
    }
}
