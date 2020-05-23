#region using directives

using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Enums;
using LeChuck.ReferralLinks.Domain.Interfaces;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services.UrlShorteners
{
    public class DefaultShortenerStrategy : IUrlShortenerStrategy
    {
        public string Name { get; } = Constants.Providers.Shorteners.None;
        public Task<string> ShortenUrl(string url)
        {
            return Task.FromResult(url);
        }

    }
}