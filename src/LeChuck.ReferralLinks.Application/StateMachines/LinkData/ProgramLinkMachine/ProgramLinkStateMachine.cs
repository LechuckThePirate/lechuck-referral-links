using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine
{
    public class ProgramLinkStateMachine : StateMachine<IUpdateContext, Domain.Models.MultiLink>
    {
        public ProgramLinkStateMachine(IStateMachineStore stateMachineStore,
            IStateMachineStrategySelector<IUpdateContext, Domain.Models.MultiLink> strategySelector,
            ILogger<ProgramLinkStateMachine> logger)
            : base(new ProgramLinkStateMachineWorkflow(), stateMachineStore, logger, strategySelector)
        { }

    }
}
