#region using directives

using LeChuck.ReferralLinks.Domain.Enums;

#endregion

namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface IUrlShortenerProvider
    {
        IUrlShortenerStrategy GetShortenerByName(string serviceName);
    }
}