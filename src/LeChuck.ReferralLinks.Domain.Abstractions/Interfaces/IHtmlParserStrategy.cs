using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface IHtmlParserStrategy
    {
        bool CanParse(string url);
        Task<LinkData> ParseUrl(string url);
    }
}