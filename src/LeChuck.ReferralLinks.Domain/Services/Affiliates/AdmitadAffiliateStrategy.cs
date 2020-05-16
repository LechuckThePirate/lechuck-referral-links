#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Services.ApiClients;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services.Affiliates
{
    public class AdmitadAffiliateStrategy : IAffiliateStrategy
    {
        private readonly IAdmitadApiClient _admitadApiClient;

        public AdmitadAffiliateStrategy(IAdmitadApiClient admitadApiClient)
        {
            _admitadApiClient = admitadApiClient ?? throw new ArgumentNullException(nameof(admitadApiClient));
        }

        public bool Handles(string url) => true;

        public async Task<string> GetCommisionedDeepLink(string url)
        {
            var newUrl = await _admitadApiClient.DeepLink(1440118, 6115, url);


            return (newUrl.Any()) ? newUrl[0] : null;
        }
    }
}