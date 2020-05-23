#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Services.ApiClients;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services.Affiliates
{
    public class AdmitadAffiliateStrategy : IAffiliateStrategy
    {
        private readonly IAdmitadApiClient _admitadApiClient;
        private readonly AffiliateConfig _affiliateConfig;

        public AdmitadAffiliateStrategy(IAdmitadApiClient admitadApiClient, AppConfiguration appConfig)
        {
            _admitadApiClient = admitadApiClient ?? throw new ArgumentNullException(nameof(admitadApiClient));
            _affiliateConfig = appConfig.AffiliateServices
                                   .FirstOrDefault(af => af.Name == this.Name)
                               ?? throw new ArgumentNullException(nameof(appConfig));
            Enabled = _affiliateConfig.Enabled;
        }

        public string Name => Constants.Providers.Affiliates.Admitad;
        public bool Enabled { get; private set; }

        // TODO: Check if it can handle a vendor
        public bool Handles(string parser) => parser == Constants.Providers.Vendors.AliExpress;

        public async Task<IEnumerable<DeepLink>> GetDeepLinks(string vendor, IEnumerable<string> urls)
        {
            var deepLinks = urls.Select((u, i) => new DeepLink(u,i)).ToArray();

            var result = await _admitadApiClient.DeepLinks(GetSpaceId(), GetCampaignId(vendor), urls);
            for (var i = 0; i < result.Length; i++)
            {
                deepLinks[i].DeepLinkUrl = result[i];
            }

            return deepLinks;
        }

        public async Task<string> GetDeepLink(string vendor, string url)
        {
            var spaceId = GetSpaceId();
            var campaignId = GetCampaignId(vendor);
            var result = await _admitadApiClient.DeepLink(spaceId, campaignId, url);
            return result;
        }

        public async Task<IEnumerable<AffiliateSpace>> GetSpaces()
        {
            return await _admitadApiClient.GetSpaces();
        }

        public string GetCampaignId(string vendor)
        {
            // TODO: Get from config
            return "6115";
        }

        private string GetSpaceId()
        {
            return _affiliateConfig.SpaceId;
        }
    }
}