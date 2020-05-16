#region using directives

using System.Threading.Tasks;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services.ApiClients
{
    public interface IAdmitadApiClient
    {
        Task<bool> Authenticate();
        Task<string[]> DeepLink(int spaceId, int campaignId, string url);
    }
}