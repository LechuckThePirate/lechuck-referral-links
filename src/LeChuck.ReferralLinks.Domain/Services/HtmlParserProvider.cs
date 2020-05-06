using System;
using System.Collections.Generic;
using System.Linq;
using LeChuck.ReferralLinks.Domain.Interfaces;

namespace LeChuck.ReferralLinks.Domain.Services
{
    public class HtmlHtmlParserProvider : IHtmlParserProvider
    {
        private readonly IEnumerable<IHtmlParserStrategy> _parsers;

        public HtmlHtmlParserProvider(IEnumerable<IHtmlParserStrategy> parsers)
        {
            _parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        }

        public IHtmlParserStrategy GetParserFor(string url)
        {
            return _parsers.FirstOrDefault(p => p.CanParse(url));
        }
    }
}
