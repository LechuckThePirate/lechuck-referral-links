using System.Runtime.InteropServices;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Domain
{
    public class Constants
    {
        public const string TelegramTokenValueName = "Config:TelegramToken";
        public const string TelegramRootUserId = "Config:RootUserId";
        public const string BitLyTokenValueName = "Config:BitLyToken";
        public static string BitLyEndpointValueName = "Config:BitLyEndpoint";

        public class MessageContentType
        {
            public const string Url = "Url";
        }

        public class EnvVarNames
        {
            public const string ConfigTable = "CONFIG_TABLE";
        }

        public const string ConfigKey = "ConfigRow";
    }
}
