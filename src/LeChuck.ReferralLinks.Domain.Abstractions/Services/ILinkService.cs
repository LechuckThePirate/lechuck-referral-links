#region using directives

using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services
{
    public interface ILinkService
    {
        Task<Link> BuildMessage(string url);
    }
}