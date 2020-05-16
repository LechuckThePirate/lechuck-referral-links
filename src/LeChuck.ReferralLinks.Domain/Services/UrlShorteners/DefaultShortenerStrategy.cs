#region using directives

using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Enums;
using LeChuck.ReferralLinks.Domain.Interfaces;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services.UrlShorteners
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