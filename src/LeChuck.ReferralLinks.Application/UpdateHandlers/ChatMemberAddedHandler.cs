using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Contracts.UnitsOfWork;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.UpdateHandlers
{
    public class ChatMemberAddedHandler : IUpdateHandler
    {
        private readonly AppConfiguration _config;
        private readonly IConfigUnitOfWork _unitOfWork;
        private readonly ILogger<ChatMemberAddedHandler> _logger;

        public ChatMemberAddedHandler(AppConfiguration config, IConfigUnitOfWork unitOfWork, ILogger<ChatMemberAddedHandler> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool CanHandle(IUpdateContext update) => 
            update.MessageType == MessageTypeEnum.ChatMemberAdded 
            && update.AffectedUserIds.Any(u => u == _config.MeId);

        public async Task HandleUpdate(IUpdateContext updateContext)
        {
            _logger.LogInformation($"The bot joined channel '{updateContext.ChatName}' ({updateContext.ChatId})");
            _config.Channels ??= new List<Channel>();
            if (_config.Channels.All(c => c.ChannelId != updateContext.ChatId.ToString()))
            {
                var channel = new Channel
                {
                    ChannelId = updateContext.ChatId.ToString(),
                    ChannelName = updateContext.ChatName
                };
                _config.Channels.Add(channel);
                _logger.LogInformation($"Added new channel to list");
                await _unitOfWork.SaveConfig(_config);
            }
        }

    }
}
