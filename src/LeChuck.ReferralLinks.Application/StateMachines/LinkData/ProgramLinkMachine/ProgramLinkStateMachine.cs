#region using directives

using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.Logging;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine
{
    public class ProgramLinkStateMachine : StateMachine<IUpdateContext, MultiLink>
    {
        public ProgramLinkStateMachine(IStateMachineStore stateMachineStore,
            IStateMachineStrategySelector<IUpdateContext, MultiLink> strategySelector,
            ILogger<ProgramLinkStateMachine> logger)
            : base(new ProgramLinkStateMachineWorkflow(), stateMachineStore, logger, strategySelector)
        {
        }
    }
}