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
    public class SetAffiliateStateCommand : IConfigStrategy
    {
        private readonly ILogger<SetAffiliateStateCommand> _logger;
        private readonly AppConfiguration _config;

        public SetAffiliateStateCommand(ILogger<SetAffiliateStateCommand> logger, AppConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string key) =>
            key == ConfigStateMachineWorkflow.CommandsEnum.SetAffiliateSpaceCmd.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            if (context.CallbackButtonData.Length < 2 || string.IsNullOrWhiteSpace(context.CallbackButtonData[1])) 
            {
                _logger.LogError("No data in callback");
                return false;
            }

            var selectedAffiliate =
                stateMachine.GetParameter<AffiliateConfig>(ConfigStateMachineWorkflow.Params.SelectedAffiliate);
            if (selectedAffiliate == null)
            {
                _logger.LogError("No affiliate selected");
                return false;
            }

            var affiliate = _config.AffiliateServices.FirstOrDefault(a => a.Name == selectedAffiliate.Name);
            if (affiliate == null)
            {
                _logger.LogError($"No affiliate {selectedAffiliate.Name} in configuration");
                return false;
            }

            affiliate.SpaceId = context.CallbackButtonData[1];
            stateMachine.SetParameter(ConfigStateMachineWorkflow.Params.SelectedAffiliate, affiliate);
            return await Task.FromResult(true);
        }
    }
}
