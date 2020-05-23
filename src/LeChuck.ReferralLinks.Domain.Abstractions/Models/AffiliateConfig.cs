using System;

namespace LeChuck.ReferralLinks.Domain.Models
{
    public class AffiliateConfig
    {
        public string Name { get; set; }
        public string AuthEndpoint { get; set; }
        public string ApiEndpoint { get; set; }
        public ApiCredentials Credentials { get; set; }
        public string SpaceId { get; set; }
        public bool Enabled { get; set; }
        public string ShortenerName { get; set; }

        public bool IsValidCredentials()
        {
            return Credentials != null && !string.IsNullOrWhiteSpace(Credentials.ClientId) &&
                   !string.IsNullOrWhiteSpace(Credentials.ClientSecret);
        }

        public bool IsValidSpace()
        {
            return !string.IsNullOrWhiteSpace(SpaceId);
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(ApiEndpoint)
                   && (string.IsNullOrWhiteSpace(AuthEndpoint) || IsValidCredentials())
                   && (IsValidSpace());
        }

        public bool ToggleEnabled()
        {
            if (Enabled || IsValid())
            {
                Enabled = !Enabled;
                return true;
            }

            return false;
        }
    }
}