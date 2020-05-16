#region using directives

using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Services;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.Extensions.Logging;

#endregion

namespace LeChuck.ReferralLinks.Application.CommandHandlers
{
    public class RegisterChannelCommandHandler : ICommandHandler
    {
        private readonly ILogger<RegisterChannelCommandHandler> _logger;
        private readonly IBotService _bot;
        private readonly IChannelService _channelService;

        public RegisterChannelCommandHandler(
            ILogger<RegisterChannelCommandHandler> logger,
            IBotService bot,
            IChannelService channelService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bot = bot;
            _channelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
        }

        public bool CanHandle(string command) => command == Commands.RegisterChannel;

        public async Task HandleCommand(IUpdateContext updateContext)
        {
            await Task.WhenAll(
                _channelService.AddBotToChannel(new Channel(updateContext.ChatId, updateContext.ChatName)),
                _bot.SendTextMessageAsync(updateContext.ChatId, "Bot registrado en el canal!")
            );
        }
    }
}