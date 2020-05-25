using System;
using System.Collections.Generic;
using System.Text;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class VendorConfig
    {
        public string Name { get; set; }
        public string AffiliateCustomizer { get; set; }
        public bool ShortenerEnabled { get; set; } = true;
        public string CustomizerPrompt { get; set; }
    }
}
