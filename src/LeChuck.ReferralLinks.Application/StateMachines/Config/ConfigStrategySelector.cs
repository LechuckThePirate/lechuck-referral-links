using System;
using System.Collections.Generic;
using System.Linq;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config
{
    public interface IConfigStrategySelector : IStateMachineStrategySelector<IUpdateContext, AppConfiguration> { }

    public class ConfigStrategySelector : IConfigStrategySelector
    {
        private readonly IEnumerable<IConfigStrategy> _strategies;

        public ConfigStrategySelector(IEnumerable<IConfigStrategy> strategies)
        {
            _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
        }

        public IStateMachineStrategy<IUpdateContext, AppConfiguration> GetHandlerFor(string selectKey)
        {
            return _strategies.FirstOrDefault(s => s.CanHandle(selectKey));
        }
    }
}
