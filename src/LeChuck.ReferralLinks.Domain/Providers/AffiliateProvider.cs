﻿#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using LeChuck.ReferralLinks.Domain.Interfaces;

#endregion

namespace LeChuck.ReferralLinks.Domain.Providers
{
    public interface IAffiliateProvider
    {
        IAffiliateStrategy GetAffiliateFor(string vendor);
        IAffiliateStrategy GetAffiliateByName(string selectedAffiliateName);
    }

    public class AffiliateProvider : IAffiliateProvider
    {
        private readonly IEnumerable<IAffiliateStrategy> _affiliateStrategies;

        public AffiliateProvider(IEnumerable<IAffiliateStrategy> affiliateStrategies)
        {
            _affiliateStrategies = affiliateStrategies ?? throw new ArgumentNullException(nameof(affiliateStrategies));
        }

        public IAffiliateStrategy GetAffiliateFor(string vendor)
        {
            return _affiliateStrategies
                .FirstOrDefault(s => s.Handles(vendor));
        }
        
        public IAffiliateStrategy GetAffiliateByName(string selectedAffiliateName)
        {
            return _affiliateStrategies.FirstOrDefault(s => s.Name == selectedAffiliateName);
        }
    }
}