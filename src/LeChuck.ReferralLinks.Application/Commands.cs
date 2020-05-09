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

        // Diagnostics
#if DEBUG
        public static string ReadUrl = "leerurl";
#endif

        public static CommandModel[] CommandModels =
        {
            new CommandModel
            {
                CommandName = Help, Enabled = true, AdminOnly = false, AvailableFor = PrivateAndChat,
                HelpString = "Ayuda del bot"
            },
#if DEBUG
            new CommandModel
            {
                CommandName = ReadUrl, Enabled = true, AdminOnly = true, AvailableFor = PrivateAndChat,
                HelpString = "(dev only - leer pagina)"
            }
#endif
        };

    }

}
