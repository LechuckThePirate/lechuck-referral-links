using LeChuck.ReferralLinks.Application.StateMachines.Strategies;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Application.StateMachines
{
    public interface ILinkDataStrategy : IStrategy<IUpdateContext, LinkData>
    {
    }
}
