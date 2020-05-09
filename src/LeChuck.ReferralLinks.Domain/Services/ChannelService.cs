using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Contracts.UnitsOfWork;
using LeChuck.ReferralLinks.Domain.Models;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Domain.Services
{
    public class ChannelService : IChannelService
    {
        private readonly ILogger<ChannelService> _logger;
        private readonly AppConfiguration _config;
        private readonly IConfigUnitOfWork _unitOfWork;

        public ChannelService(ILogger<ChannelService> logger, AppConfiguration config, IConfigUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task AddBotToChannel(Channel channel)
        {
            _config.Channels ??= new List<Channel>();
            if (_config.Channels.All(c => c.ChannelId != channel.ChannelId))
            {
                _config.Channels.Add(channel);
                _logger.LogInformation($"Added new channel to list");
                await _unitOfWork.SaveConfig(_config);
            }
        }

        public async Task RemoveBotFromChannel(Channel channel)
        {
            _config.Channels ??= new List<Channel>();
            if (_config.Channels.All(c => c.ChannelId != channel.ChannelId))
                return;

            _config.Channels.RemoveAll(c => c.ChannelId == channel.ChannelId);

            _logger.LogInformation($"Removed channel from list");
            await _unitOfWork.SaveConfig(_config);
        }
    }
}
