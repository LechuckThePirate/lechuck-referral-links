#region using directives

using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData
{
    public interface IMultiLinkStrategy : IStateMachineStrategy<IUpdateContext, MultiLink>
    {
    }
}