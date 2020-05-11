using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Application.CommandHandlers
{
    public class ConfigCommandHandler : ICommandHandler
    {
        private readonly IStateMachineFactory _machineFactory;
        private readonly AppConfiguration _config;

        public ConfigCommandHandler(IStateMachineFactory machineFactory, AppConfiguration config)
        {
            _machineFactory = machineFactory ?? throw new ArgumentNullException(nameof(machineFactory));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string command) => command == Commands.Config;

        public async Task HandleCommand(IUpdateContext updateContext)
        {
            var machine = await _machineFactory.Create<IUpdateContext, AppConfiguration>(updateContext.User.UserId.ToString());
            await machine.Run(context: updateContext, entity: _config);
        }
    }
}
