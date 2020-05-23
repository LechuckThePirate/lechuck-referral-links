#region using directives

using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

#endregion

namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface ILinkParserStrategy
    {
        public string Name { get; }
        bool CanParse(string content);
        Task<LinkMessage> ParseContent(string content);
    }
}