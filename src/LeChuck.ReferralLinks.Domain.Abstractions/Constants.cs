namespace LeChuck.ReferralLinks.Domain
{
    public class Constants
    {
        public const string TelegramTokenValueName = "BOT_KEY";

        public const string TelegramRootUserId = "ROOT_USER_ID";
        public const string BitLyTokenValueName = "BITLY_TOKEN";
        public static string BitLyEndpointValueName = "BITLY_ENDPOINT";

        public class Providers
        {
            public class Shorteners
            {
                public const string BitLy = nameof(BitLy);
                public const string None = nameof(None);
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