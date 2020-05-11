using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.CommandHandlers
{
    public class ProgramLinkCommandHandler : ICommandHandler
    {
        private readonly ILogger<ProgramLinkCommandHandler> _logger;
        private readonly IStateMachineFactory _machineFactory;

        public ProgramLinkCommandHandler(ILogger<ProgramLinkCommandHandler> logger, IStateMachineFactory machineFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _machineFactory = machineFactory;
        }

        public bool CanHandle(string command) => command == Commands.ProgramLink;

        public async Task HandleCommand(IUpdateContext updateContext)
        {
            var machine = await _machineFactory.Create<IUpdateContext, MultiLink>(updateContext.User.UserId.ToString());
            await machine.Run(null, updateContext);
        }
    }
}
