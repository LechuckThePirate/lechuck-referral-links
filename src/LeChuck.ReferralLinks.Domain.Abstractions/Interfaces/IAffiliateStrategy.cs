using System.Threading.Tasks;

namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface IAffiliateStrategy
    {
        bool Handles(string url);
        Task<string> GetCommisionedDeepLink(string url);
    }
}