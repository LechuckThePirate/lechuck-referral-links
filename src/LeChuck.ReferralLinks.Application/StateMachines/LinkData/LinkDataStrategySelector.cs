using System;
using System.Collections.Generic;
using System.Linq;
using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData
{
    public interface ILinkDataStrategySelector : IStateMachineStrategySelector<IUpdateContext, Domain.Models.MultiLink> { }

    public class LinkDataStrategySelector : ILinkDataStrategySelector
    {
        private readonly IEnumerable<IMultiLinkStrategy> _strategies;

        public LinkDataStrategySelector(IEnumerable<IMultiLinkStrategy> strategies)
        {
            _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
        }

        public IStateMachineStrategy<IUpdateContext, Domain.Models.MultiLink> GetHandlerFor(string selectKey)
        {
            return _strategies.FirstOrDefault(s => s.CanHandle(selectKey));
        }
    }
}
