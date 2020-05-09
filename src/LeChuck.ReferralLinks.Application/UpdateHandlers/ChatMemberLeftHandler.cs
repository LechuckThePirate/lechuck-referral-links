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
    public class ChatMemberLeftHandler : IUpdateHandler
    {
        private readonly AppConfiguration _config;
        private readonly IConfigUnitOfWork _unitOfWork;
        private readonly ILogger<ChatMemberLeftHandler> _logger;

        public ChatMemberLeftHandler(AppConfiguration config, IConfigUnitOfWork unitOfWork, ILogger<ChatMemberLeftHandler> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool CanHandle(IUpdateContext update) =>
            update.MessageType == MessageTypeEnum.ChatMemberLeft
            && update.AffectedUserIds.Any(u => u == _config.MeId);

        public async Task HandleUpdate(IUpdateContext updateContext)
        {
            _logger.LogInformation($"The bot left channel '{updateContext.ChatName}' ({updateContext.ChatId})");

            _config.Channels ??= new List<Channel>();
            if (_config.Channels.All(c => c.ChannelId != updateContext.ChatId.ToString()))
                return;

            _config.Channels.RemoveAll(c => c.ChannelId == updateContext.ChatId.ToString());

            _logger.LogInformation($"Removed channel from list");
            await _unitOfWork.SaveConfig(_config);
        }

    }
}
