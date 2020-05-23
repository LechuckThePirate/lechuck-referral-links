﻿#region using directives

using System.Collections.Generic;
using System.Linq;

#endregion

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class AppConfiguration
    {
        public long MeId { get; set; }
        public string CommandPrefix { get; set; } = "/";
        public string RootUserId { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public List<Channel> Channels { get; set; } = new List<Channel>();
        public List<AffiliateConfig> AffiliateServices { get; set; } = new List<AffiliateConfig>();
        public string DefaultShortener { get; set; }

        public AffiliateConfig GetAffiliateConfig(string affiliate)
            => AffiliateServices.FirstOrDefault(a => a.Name == affiliate);

    }
}