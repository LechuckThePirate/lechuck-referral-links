#region using directives

using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Models;

#endregion

namespace LeChuck.ReferralLinks.Application
{
    public static class Commands
    {
        public static CommandSourceEnum[] PrivateOnly = {CommandSourceEnum.Private};
        public static CommandSourceEnum[] ChatOnly = {CommandSourceEnum.Chat};
        public static CommandSourceEnum[] PrivateAndChat = {CommandSourceEnum.Private, CommandSourceEnum.Chat};
        public static CommandSourceEnum[] ChannelOnly = {CommandSourceEnum.Channel};

        public static UserTypeEnum[] AllUsers = {UserTypeEnum.Root, UserTypeEnum.Admin, UserTypeEnum.RegularUser};
        public static UserTypeEnum[] RegularUserOnly = {UserTypeEnum.RegularUser};
        public static UserTypeEnum[] AdminOnly = {UserTypeEnum.Root, UserTypeEnum.Admin};
        public static UserTypeEnum[] RootOnly = {UserTypeEnum.Root};

#if DEBUG
        public static string ReadUrl = "leerurl";
#endif

        public static string Help = "ayuda";
        public static string Broadcast = "enviar";
        public static string RegisterChannel = "añadircanal";
        public static string ProgramLink = "programar";
        public static string RegisterUser = "nuevoadmin";
        public static string WhoAmI = "quiensoy";
        public static string Config = "config";

        public static CommandModel[] CommandModels =
        {
            new CommandModel
            {
                CommandName = Help, Enabled = true, AllowedFor = AllUsers, AvailableFor = PrivateAndChat,
                HelpString = "Ayuda del bot"
            },
            new CommandModel
            {
                CommandName = Broadcast, Enabled = true, AllowedFor = AdminOnly, AvailableFor = PrivateOnly,
                HelpString = "Enviar publicaciones a canales"
            },
            new CommandModel
            {
                CommandName = RegisterChannel, Enabled = true, AllowedFor = AdminOnly, AvailableFor = ChannelOnly,
                HelpString = "Registrar al bot en un canal"
            },
            new CommandModel
            {
                CommandName = ProgramLink, Enabled = true, AllowedFor = AdminOnly, AvailableFor = PrivateOnly,
                HelpString = "Programar un link"
            },
            new CommandModel
            {
                CommandName = RegisterUser, Enabled = true, AllowedFor = RootOnly, AvailableFor = PrivateOnly,
                HelpString = "Añadir un administrador"
            },
            new CommandModel
            {
                CommandName = WhoAmI, Enabled = true, AllowedFor = AllUsers, AvailableFor = PrivateOnly,
                HelpString = "Tu info de usuario"
            },
            new CommandModel
            {
                CommandName = Config, Enabled = true, AllowedFor = AdminOnly, AvailableFor = PrivateOnly,
                HelpString = "Configurar el bot"
            },
#if DEBUG
            new CommandModel
            {
                CommandName = ReadUrl, Enabled = true, AllowedFor = RootOnly, AvailableFor = PrivateAndChat,
                HelpString = "(dev only - leer pagina)"
            }
#endif
        };
    }
}