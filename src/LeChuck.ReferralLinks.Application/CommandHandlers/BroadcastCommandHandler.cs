#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.Views;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Services;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.Extensions.Logging;

#endregion

namespace LeChuck.ReferralLinks.Application.CommandHandlers
{
    public class BroadcastCommandHandler : ICommandHandler
    {
        private readonly ILogger<BroadcastCommandHandler> _logger;
        private readonly IBotService _bot;
        private readonly ILinkService _linkService;
        private readonly ILinkView _linkView;
        private readonly AppConfiguration _configuration;

        public BroadcastCommandHandler(
            ILogger<BroadcastCommandHandler> logger,
            IBotService bot,
            ILinkService linkService,
            ILinkView linkView,
            AppConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _linkService = linkService ?? throw new ArgumentNullException(nameof(linkService));
            _linkView = linkView ?? throw new ArgumentNullException(nameof(linkView));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public bool CanHandle(string command) => command == Commands.Broadcast;

        public async Task HandleCommand(IUpdateContext updateContext)
        {
            var url = updateContext.MessageText?.Split(" ");
            if (url.Length < 2)
            {
                await _bot.SendTextMessageAsync(updateContext.ChatId,
                    $"Debes incluir un enlace. Ejemplo: {_configuration.CommandPrefix}{Commands.Broadcast} https://www.test.com");
                return;
            }

            var message = await _linkService.BuildMessage(url[1]);
            var tasks = _configuration.Channels.Select(c =>
            {
                try
                {
                    _logger.LogInformation($"Sending Link {message} to {c}");
                    return _linkView.SendView(c.ChannelId, message);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Can't send to channel {c}: {ex.Message}\n{ex.StackTrace}");
                    return Task.CompletedTask;
                }
            });
            await Task.WhenAll(tasks);
        }
    }
}