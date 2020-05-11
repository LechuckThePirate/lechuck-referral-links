using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData
{
    public interface IMultiLinkStrategy : IStateMachineStrategy<IUpdateContext, Domain.Models.MultiLink>
    {
    }
}
