using System;
using System.Collections.Generic;
using System.Text;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class DeepLink
    {
        public int Number { get; set; }
        public string Url { get; set; }
        public string DeepLinkUrl { get; set; }

        public DeepLink() { }
        public DeepLink(string url, int number)
        {
            Url = url;
            Number = number;
        }
    }
}
