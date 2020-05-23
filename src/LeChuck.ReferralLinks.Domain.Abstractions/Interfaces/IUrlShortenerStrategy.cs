#region using directives

using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Enums;

#endregion

namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface IUrlShortenerStrategy
    {
        Task<string> ShortenUrl(string url);

        string Name { get; }
    }
}