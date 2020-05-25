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
    public class SelectVendorCommand  : IConfigStrategy
    {
        private readonly ILogger<SelectVendorCommand> _logger;
        private readonly AppConfiguration _config;

        public SelectVendorCommand(ILogger<SelectVendorCommand> logger, AppConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string key) =>
            key == ConfigStateMachineWorkflow.CommandsEnum.SelectVendorCmd.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            if (context.CallbackButtonData.Length < 2)
            {
                _logger.LogError("No data in callback");
                return false;
            }

            var selectedVendorName = context.CallbackButtonData[1];
            var selectedVendor = _config.VendorServices.FirstOrDefault(vnd => vnd.Name == selectedVendorName);
            if (selectedVendor == null)
            {
                _logger.LogError($"Vendor {selectedVendorName} not found.");
                return false;
            }

            stateMachine.SetParameter(ConfigStateMachineWorkflow.Params.SelectedVendor, selectedVendor);
            return await Task.FromResult(true);
        }
    }
}
