#region using directives

using System.Threading.Tasks;

#endregion

namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface IAffiliateStrategy
    {
        bool Handles(string url);
        Task<string> GetCommisionedDeepLink(string url);
    }
}