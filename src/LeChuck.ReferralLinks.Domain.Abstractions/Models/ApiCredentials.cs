#region using directives

using System;

#endregion

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class ApiCredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthType { get; set; } = "Basic";
        public string TokenType { get; set; } = "Bearer";
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? Expires { get; set; }
    }
}