using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Models;

namespace LeChuck.ReferralLinks.Application
{
    public static class Commands
    {

        public static CommandSourceEnum[] PrivateOnly = new[] { CommandSourceEnum.Private };
        public static CommandSourceEnum[] ChatOnly = new[] { CommandSourceEnum.Chat };
        public static CommandSourceEnum[] PrivateAndChat = new[] { CommandSourceEnum.Private, CommandSourceEnum.Chat };

        public static string Help = "ayuda";

        public static CommandModel[] CommandModels =
        {
            new CommandModel
            {
                CommandName = Help, Enabled = true, AdminOnly = false, AvailableFor = PrivateAndChat,
                HelpString = "Ayuda del bot"
            }
        };

    }

}
