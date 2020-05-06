using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Enums;
using LeChuck.ReferralLinks.Domain.Interfaces;

namespace LeChuck.ReferralLinks.Application.Services.UrlShorteners
{
    public class DefaultShortenerStrategy : IUrlShortenerStrategy
    {
        public UrlShortenersEnum Key { get; }

        public Task<string> ShortenUrl(string url)
        {
            // Placeholder class for No shortening, 
            // used for fallback as well for non-existing
            // shorteners
            return Task.FromResult(url);
        }
    }
}
