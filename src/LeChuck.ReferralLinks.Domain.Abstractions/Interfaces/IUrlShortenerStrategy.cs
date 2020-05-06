using System.Threading.Tasks;

namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface IUrlShortenerStrategy
    {
        Enums.UrlShortenersEnum Key => Enums.UrlShortenersEnum.None;
        Task<string> ShortenUrl(string url);
    }
}