#region using directives

using System.Collections.Generic;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

#endregion

namespace LeChuck.ReferralLinks.Domain.Interfaces
{
    public interface IAffiliateStrategy
    {
        string Name { get; }
        bool Handles(string parser);
        Task<IEnumerable<DeepLink>> GetDeepLinks(string vendor, IEnumerable<string> urls);
        Task<string> GetDeepLink(string vendor, string url);
    }
}