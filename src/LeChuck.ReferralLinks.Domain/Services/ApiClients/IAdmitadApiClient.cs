using System.Threading.Tasks;

namespace LeChuck.ReferralLinks.Domain.Services.ApiClients
{
    public interface IAdmitadApiClient
    {
        Task<bool> Authenticate();
        Task<string[]> DeepLink(int spaceId, int campaignId, string url);
    }
}