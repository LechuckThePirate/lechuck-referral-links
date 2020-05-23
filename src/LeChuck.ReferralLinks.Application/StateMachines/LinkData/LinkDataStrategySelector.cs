#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData
{
    public interface ILinkDataStrategySelector : IStateMachineStrategySelector<IUpdateContext, MultiLinkMessage>
    {
    }

    public class LinkDataStrategySelector : ILinkDataStrategySelector
    {
        private readonly IEnumerable<IMultiLinkStrategy> _strategies;

        public LinkDataStrategySelector(IEnumerable<IMultiLinkStrategy> strategies)
        {
            _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
        }

        public IStateMachineStrategy<IUpdateContext, MultiLinkMessage> GetHandlerFor(string selectKey)
        {
            return _strategies.FirstOrDefault(s => s.CanHandle(selectKey));
        }
    }
}