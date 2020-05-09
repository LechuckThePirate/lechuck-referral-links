using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Domain.Services
{
    public interface ILinkService
    {
        Task<LinkData> BuildMessage(string url);
    }
}