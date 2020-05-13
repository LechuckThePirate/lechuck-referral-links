using System.Collections.Generic;
using System.Linq;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class AppConfiguration
    {
        public long MeId { get; set; }
        public string CommandPrefix { get; set; } = "/";
        public string RootUserId { get; set; }
        public List<User> Users { get; set; }
        public List<Channel> Channels { get; set; }
        public List<AffiliateServiceConfig> AffiliateServices { get; set; } = new List<AffiliateServiceConfig>();

        public AffiliateServiceConfig GetAffiliateConfig(string affiliate) 
            => AffiliateServices.FirstOrDefault(a => a.Service == affiliate);
    }
}
