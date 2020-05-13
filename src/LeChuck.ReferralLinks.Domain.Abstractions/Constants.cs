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

        public class Providers
        {
            public class Affiliates
            {
                public const string Admitad = nameof(Admitad);
                public const string Amazon = nameof(Amazon);
            }

            public class Shorteners
            {
                public const string BitLy = nameof(BitLy);
            }

            public class Vendors
            {
                public const string AliExpress = nameof(AliExpress);
                public const string Amazon = nameof(Amazon);
                public const string BangGood = nameof(BangGood);
                public const string PatPat = nameof(PatPat);
            }
        }

        public class MessageContentType
        {
            public const string Url = "Url";
        }

        public class EnvVarNames
        {
            public const string ConfigTable = "CONFIG_TABLE";
            public const string TimedTasksTableName = "TIMED_TASKS_TABLE";
            public const string LinkDataTableName = "LINK_DATA_TABLE";
            public const string StateMachineTableName = "STATE_MACHINE_TABLE";
        }

        public const string ConfigKey = "ConfigRow";
    }
}
