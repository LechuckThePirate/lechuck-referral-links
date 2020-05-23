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
    public class SetVendorGotoLinkCommand : IConfigStrategy
    {
        private readonly ILogger<SetVendorGotoLinkCommand> _logger;
        private readonly AppConfiguration _config;

        public SetVendorGotoLinkCommand(ILogger<SetVendorGotoLinkCommand> logger, AppConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string key) => key == ConfigStateMachineWorkflow.CommandsEnum.SetVendorGotoLinkCmd.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            var vendor = stateMachine.GetParameter<VendorConfig>(ConfigStateMachineWorkflow.Params.SelectedVendor);
            if (vendor == null)
            {
                _logger.LogError("No vendor selected");
                return false;
            }
            vendor.GotoLink = context.MessageText;

            stateMachine.SetParameter(ConfigStateMachineWorkflow.Params.SelectedVendor, vendor);
            var provider = _config.VendorServices.FirstOrDefault(vnd => vnd.Name == vendor.Name);
            if (provider != null)
            {
                provider.GotoLink = vendor.GotoLink;
            }

            return await Task.FromResult(true);
        }
    }
}
