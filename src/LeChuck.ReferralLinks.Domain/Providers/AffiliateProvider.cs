using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeChuck.ReferralLinks.Domain.Interfaces;

namespace LeChuck.ReferralLinks.Domain.Providers
{
    public interface IAffiliateProvider
    {
        IAffiliateStrategy GetAffiliateFor(string url);
    }

    public class AffiliateProvider : IAffiliateProvider
    {
        private readonly IEnumerable<IAffiliateStrategy> _affiliateStrategies;

        public AffiliateProvider(IEnumerable<IAffiliateStrategy> affiliateStrategies)
        {
            _affiliateStrategies = affiliateStrategies ?? throw new ArgumentNullException(nameof(affiliateStrategies));
        }

        public IAffiliateStrategy GetAffiliateFor(string url)
        {
            return _affiliateStrategies.FirstOrDefault(s => s.Handles(url));
        }
    }
}
