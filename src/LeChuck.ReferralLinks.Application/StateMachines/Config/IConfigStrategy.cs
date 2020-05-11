using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config
{
    public interface IConfigStrategy : IStateMachineStrategy<IUpdateContext, AppConfiguration> { }
}
