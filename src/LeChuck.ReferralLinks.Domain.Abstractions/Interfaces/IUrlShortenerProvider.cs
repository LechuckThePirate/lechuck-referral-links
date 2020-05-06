using LeChuck.ReferralLinks.Domain.Enums;

namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface IUrlShortenerProvider
    {
        IUrlShortenerStrategy GetServiceOrDefault(UrlShortenersEnum serviceName);
    }
}