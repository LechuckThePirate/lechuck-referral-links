using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Commands
{
    public class SelectAffiliateCommand  : IConfigStrategy
    {
        private readonly ILogger<SelectAffiliateCommand> _logger;
        private readonly AppConfiguration _config;

        public SelectAffiliateCommand(ILogger<SelectAffiliateCommand> logger, AppConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string key) =>
            key == ConfigStateMachineWorkflow.CommandsEnum.SelectAffiliateCmd.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            if (context.CallbackButtonData.Length < 2)
            {
                _logger.LogError("No data in callback");
                return false;
            }

            var selectedAffiliateName = context.CallbackButtonData[1];
            var selectedAffiliate = _config.AffiliateServices.FirstOrDefault(aff => aff.Name == selectedAffiliateName);
            if (selectedAffiliate == null)
            {
                _logger.LogError($"Affiliate {selectedAffiliateName} not found.");
                return false;
            }

            stateMachine.SetParameter(ConfigStateMachineWorkflow.Params.SelectedAffiliate, selectedAffiliate);
            return await Task.FromResult(true);
        }
    }
}
