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
    public class SetClientSecretCommand : IConfigStrategy
    {
        private readonly ILogger<SetClientSecretCommand> _logger;
        private readonly AppConfiguration _config;

        public SetClientSecretCommand(ILogger<SetClientSecretCommand> logger, AppConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string key) => key == ConfigStateMachineWorkflow.CommandsEnum.SetClientSecretCmd.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            var affiliate =
                stateMachine.GetParameter<AffiliateConfig>(ConfigStateMachineWorkflow.Params.SelectedAffiliate);
            if (affiliate == null)
            {
                _logger.LogError("No affiliate selected");
                return false;
            }
            affiliate.Credentials ??= new ApiCredentials();
            affiliate.Credentials.ClientSecret = context.MessageText;

            stateMachine.SetParameter(ConfigStateMachineWorkflow.Params.SelectedAffiliate, affiliate);
            var provider = _config.AffiliateServices.FirstOrDefault(aff => aff.Name == affiliate.Name);
            if (provider != null)
            {
                provider.Credentials = affiliate.Credentials;
            }

            return await Task.FromResult(true);
        }
    }
}
