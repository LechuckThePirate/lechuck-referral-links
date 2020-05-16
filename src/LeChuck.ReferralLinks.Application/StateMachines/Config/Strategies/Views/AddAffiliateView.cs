#region using directives

using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Views
{
    public class AddAffiliateView : IConfigStrategy
    {
        public bool CanHandle(string key) => key == ConfigStateMachineWorkflow.StatesEnum.AddAffiliateState.ToString();

        public Task<bool> Handle(IUpdateContext context, AppConfiguration entity,
            IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            throw new NotImplementedException();
        }
    }
}