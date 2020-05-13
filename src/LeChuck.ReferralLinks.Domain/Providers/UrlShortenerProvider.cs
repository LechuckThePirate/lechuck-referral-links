using System;
using System.Collections.Generic;
using System.Linq;
using LeChuck.ReferralLinks.Domain.Enums;
using LeChuck.ReferralLinks.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Domain.Providers
{
    public class UrlShortenerProvider :IUrlShortenerProvider
    {
        private readonly IEnumerable<IUrlShortenerStrategy> _shorteners;
        private readonly ILogger<IUrlShortenerProvider> _logger;

        public UrlShortenerProvider(IEnumerable<IUrlShortenerStrategy> shorteners, ILogger<IUrlShortenerProvider> logger)
        {
            _shorteners = shorteners ?? throw new ArgumentNullException(nameof(shorteners));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public IUrlShortenerStrategy GetServiceOrDefault(UrlShortenersEnum serviceName)
        {
            var result = _shorteners.FirstOrDefault(s => s.Key == serviceName);
            if (result == null)
            {
                _logger.LogError($"No shortener for key '{serviceName}");
                // Fallback to "No shortener"
                return _shorteners.First(s => s.Key == UrlShortenersEnum.None);
            }

            return result;
        }
    }
}
