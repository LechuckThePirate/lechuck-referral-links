using System;
using System.Collections.Generic;
using System.Text;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class ApiCredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthType { get; set; }
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? Expires { get; set; }
    }
}
