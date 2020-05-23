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
        private readonly ILogger<IUrlShortenerProvider> _logger;

        public UrlShortenerProvider(IEnumerable<IUrlShortenerStrategy> shorteners,
            ILogger<IUrlShortenerProvider> logger)
        {
            _shorteners = shorteners ?? throw new ArgumentNullException(nameof(shorteners));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IUrlShortenerStrategy GetShortenerByName(string serviceName)
        {
            var result = _shorteners.FirstOrDefault(s => s.Name == serviceName);
            return result;
        }
    }
}