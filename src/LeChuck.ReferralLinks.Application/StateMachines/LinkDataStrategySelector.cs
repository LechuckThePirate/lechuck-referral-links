using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeChuck.ReferralLinks.Application.StateMachines.Strategies;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Application.StateMachines.ProgramLink
{
    public interface ILinkDataStrategySelector : IStepStrategySelector<IUpdateContext, LinkData> { }

    public class LinkDataStrategySelector : ILinkDataStrategySelector
    {
        private readonly IEnumerable<ILinkDataStrategy> _strategies;

        public LinkDataStrategySelector(IEnumerable<ILinkDataStrategy> strategies)
        {
            _strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
        }

        public IStrategy<IUpdateContext, LinkData> GetHandlerFor(string selectKey)
        {
            return _strategies.FirstOrDefault(s => s.CanHandle(selectKey));
        }
    }
}
