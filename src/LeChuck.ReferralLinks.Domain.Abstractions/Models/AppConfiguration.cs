using System.Collections.Generic;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class AppConfiguration
    {
        public long MeId { get; set; }
        public string CommandPrefix { get; set; } = "/";
        public string RootUserId { get; set; }
        public List<User> Users { get; set; }
        public List<Channel> Channels { get; set; }
    }
}
