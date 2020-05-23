using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.UnitsOfWork;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Commands
{
    public class SetClientIdCommand : IConfigStrategy
    {
        private readonly ILogger<SetClientIdCommand> _logger;
        private readonly IConfigUnitOfWork _uow;
        private readonly AppConfiguration _config;

        public SetClientIdCommand(ILogger<SetClientIdCommand> logger, IConfigUnitOfWork uow, AppConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string key) => key == ConfigStateMachineWorkflow.CommandsEnum.SetClientIdCmd.ToString();

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
            affiliate.Credentials.ClientId = context.MessageText;

            stateMachine.SetParameter(ConfigStateMachineWorkflow.Params.SelectedAffiliate, affiliate);
            var provider = _config.AffiliateServices.FirstOrDefault(aff => aff.Name == affiliate.Name);
            if (provider != null)
            {
                provider.Credentials = affiliate.Credentials;
            }

            return true;
        }
    }
}
