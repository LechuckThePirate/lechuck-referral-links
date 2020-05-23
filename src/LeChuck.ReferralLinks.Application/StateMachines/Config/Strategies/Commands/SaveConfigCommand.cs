using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.UnitsOfWork;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Commands
{
    public class SaveConfigCommand : IConfigStrategy
    {
        private readonly AppConfiguration _config;
        private readonly IConfigUnitOfWork _uow;

        public SaveConfigCommand(AppConfiguration config, IConfigUnitOfWork uow)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public bool CanHandle(string key) => key == ConfigStateMachineWorkflow.CommandsEnum.SaveConfigCmd.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            await _uow.SaveConfig(_config);
            return true;
        }
    }
}
