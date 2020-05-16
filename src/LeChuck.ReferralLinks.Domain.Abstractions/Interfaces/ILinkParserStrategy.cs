#region using directives

using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

#endregion

namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface ILinkParserStrategy
    {
        public string ParserName { get; }
        bool CanParse(string content);
        Task<Link> ParseContent(string content);
    }
}