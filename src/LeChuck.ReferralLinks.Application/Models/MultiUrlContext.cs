using System;
using System.Collections.Generic;
using System.Text;

namespace LeChuck.ReferralLinks.Application.Models
{
    public class MultiUrlContext
    {
        public List<UrlContext> UrlContexts { get; set; } = new List<UrlContext>();
    }
}
