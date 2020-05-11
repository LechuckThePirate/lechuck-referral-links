using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine
{
    public class ConfigStateMachine : StateMachine<IUpdateContext, AppConfiguration>
    {
        public ConfigStateMachine(
            ILogger<ConfigStateMachine> logger,
            IStateMachineStore stateMachineStore,
            IStateMachineStrategySelector<IUpdateContext,AppConfiguration> strategySelector)
            : base(new ConfigStateMachineWorkflow(), stateMachineStore, logger, strategySelector)
        { }

        public override void DeserializeData(string data)
        {
            base.DeserializeData(data);
        }

        public override string SerializeData()
        {
            return base.SerializeData();
        }
    }
}
