using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface ILinkParserStrategy
    {
        bool CanParse(string url);
        Task<Link> ParseUrl(string url);
    }
}