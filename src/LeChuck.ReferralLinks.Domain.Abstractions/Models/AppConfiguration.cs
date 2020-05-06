using System.Collections.Generic;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class AppConfiguration
    {
        public string CommandPrefix { get; set; } = "/";
        public IEnumerable<Channel> Channels { get; set; }
    }
}
