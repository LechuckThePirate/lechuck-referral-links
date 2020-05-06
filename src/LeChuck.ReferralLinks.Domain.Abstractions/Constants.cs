using System.Runtime.InteropServices;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Domain
{
    public class Constants
    {
        public const string TelegramTokenValueName = "Config:TelegramToken";
        public const string BitLyTokenValueName = "Config:BitLyToken";
        public static string BitLyEndpointValueName = "Config:BitLyEndpoint";
        public const string ConfigKey = nameof(AppConfiguration);

        public class MessageContentType
        {
            public const string Url = "Url";
        }

        public class EnvVarNames
        {
            public const string ConfigTable = "CONFIG_TABLE";
        }

    }
}
