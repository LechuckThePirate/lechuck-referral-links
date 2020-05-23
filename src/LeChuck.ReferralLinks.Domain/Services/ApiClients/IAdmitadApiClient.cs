#region using directives

using System.Collections.Generic;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services.ApiClients
{
    public interface IAdmitadApiClient
    {
        Task<bool> Authenticate();
        Task<string[]> DeepLinks(string spaceId, string campaignId, IEnumerable<string> urls);
        Task<string> DeepLink(string spaceId, string campaignId, string urls);
        Task<IEnumerable<AffiliateSpace>> GetSpaces();
    }
}