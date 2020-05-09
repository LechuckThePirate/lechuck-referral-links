using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Contracts.UnitsOfWork;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Services;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.UpdateHandlers
{
    public class ChatMemberAddedHandler : IUpdateHandler
    {
        private readonly AppConfiguration _config;
        private readonly IChannelService _channelService;
        private readonly ILogger<ChatMemberAddedHandler> _logger;

        public ChatMemberAddedHandler(AppConfiguration config, IChannelService channelService, ILogger<ChatMemberAddedHandler> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _channelService = channelService ?? throw new ArgumentNullException(nameof(channelService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool CanHandle(IUpdateContext update) => 
            update.MessageType == MessageTypeEnum.ChatMemberAdded 
            && update.AffectedUserIds.Any(u => u == _config.MeId);

        public async Task HandleUpdate(IUpdateContext updateContext)
        {
            var channel = new Channel(updateContext.ChatId, updateContext.ChatName);
            _logger.LogInformation($"The bot joined channel {channel}");
            await _channelService.AddBotToChannel(channel);
        }

    }
}
