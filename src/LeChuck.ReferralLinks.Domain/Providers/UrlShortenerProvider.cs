#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using LeChuck.ReferralLinks.Domain.Enums;
using LeChuck.ReferralLinks.Domain.Interfaces;
using Microsoft.Extensions.Logging;

#endregion

namespace LeChuck.ReferralLinks.Domain.Providers
{
    public class UrlShortenerProvider : IUrlShortenerProvider
    {
        private readonly IEnumerable<IUrlShortenerStrategy> _shorteners;

        public UrlShortenerProvider(IEnumerable<IUrlShortenerStrategy> shorteners)
        {
            _shorteners = shorteners ?? throw new ArgumentNullException(nameof(shorteners));
        }

        public IUrlShortenerStrategy GetShortenerByName(string serviceName)
        {
            var result = _shorteners.FirstOrDefault(s => s.Name == serviceName);
            return result;
        }
    }
}