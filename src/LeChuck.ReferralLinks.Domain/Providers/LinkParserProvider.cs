#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using LeChuck.ReferralLinks.Domain.Interfaces;

#endregion

namespace LeChuck.ReferralLinks.Domain.Providers
{
    public class LinkParserProvider : ILinkParserProvider
    {
        private readonly IEnumerable<ILinkParserStrategy> _parsers;

        public LinkParserProvider(IEnumerable<ILinkParserStrategy> parsers)
        {
            _parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        public ILinkParserStrategy GetParserFor(string content)
        {
            return _parsers.FirstOrDefault(p => p.CanParse(content));
        }
    }
}