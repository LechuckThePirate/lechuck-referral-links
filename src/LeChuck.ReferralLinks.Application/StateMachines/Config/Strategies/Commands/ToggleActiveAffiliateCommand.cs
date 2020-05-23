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
    public class ToggleActiveAffiliateCommand : IConfigStrategy
    {
        private readonly ILogger<ToggleActiveAffiliateCommand> _logger;
        private readonly AppConfiguration _config;

        public ToggleActiveAffiliateCommand(ILogger<ToggleActiveAffiliateCommand> logger,
            AppConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string key) =>
            key == ConfigStateMachineWorkflow.CommandsEnum.ToggleActiveAffiliateCmd.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            var selectedAffiliate =
                stateMachine.GetParameter<AffiliateConfig>(ConfigStateMachineWorkflow.Params.SelectedAffiliate);
            if (selectedAffiliate == null)
            {
                _logger.LogError("No affiliate selected");
                return false;
            }

            var success = selectedAffiliate.ToggleEnabled();
            if (success)
            {
                stateMachine.SetParameter(ConfigStateMachineWorkflow.Params.SelectedAffiliate, selectedAffiliate);
                var configAffiliate = _config.AffiliateServices.FirstOrDefault(af => af.Name == selectedAffiliate.Name);
                if (configAffiliate != null)
                {
                    configAffiliate.Enabled = selectedAffiliate.Enabled;
                }
            }

            return await Task.FromResult(true);
        }
    }
}
