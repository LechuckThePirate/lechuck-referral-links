using System;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.ProgramLink;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.StateMachines.Strategies.Commands
{
    public class AddChannelCommandStrategy : ILinkDataStrategy
    {
        private readonly IBotService _bot;
        private readonly ILogger<AddChannelCommandStrategy> _logger;
        private readonly AppConfiguration _config;

        public AddChannelCommandStrategy(IBotService bot, ILogger<AddChannelCommandStrategy> logger, AppConfiguration config)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string key) =>
            key == ProgramLinkStateMachineWorkflow.CommandsEnum.AddChannelCmd.ToString();

        public async Task<bool> Handle(IUpdateContext context, LinkData entity)
        {
            if (!long.TryParse(context.CallbackButtonData[1], out long channelId))
            {
                _logger.LogError($"Channel {context.CallbackButtonData[1]} not in the right format!");
                return false;
            }

            var channel = _config.Channels.FirstOrDefault(c => c.ChannelId == channelId);
            if (channel == null)
            {
                _logger.LogError($"Channel {channelId} not found in config!");
                await _bot.SendTextMessageAsync(context.ChatId,"El bot no esta en ese canal, o ha habido algun problema");
                return false;
            }

            if (entity.Channels.All(c => c.ChannelId != channel.ChannelId))
                entity.Channels.Add(channel);

            _logger.LogInformation($"Added channel {channel}");

            return true;
        }
    }
}
