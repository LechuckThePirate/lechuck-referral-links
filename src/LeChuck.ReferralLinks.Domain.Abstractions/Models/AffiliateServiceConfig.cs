using System;
using System.Collections.Generic;
using System.Text;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class AffiliateServiceConfig
    {
        public string Service { get; set; }
        public string AuthEndpoint { get; set; }
        public string ApiEndpoint { get; set; }
        public ApiCredentials Credentials { get; set; }
    }
}
