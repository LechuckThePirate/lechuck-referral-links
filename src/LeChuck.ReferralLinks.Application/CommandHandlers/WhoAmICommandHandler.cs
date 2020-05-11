using System;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.CommandHandlers
{
    public class WhoAmICommandHandler : ICommandHandler
    {
        private readonly IBotService _botService;
        private readonly IBotAuthorizer _botAuthorizer;
        private readonly AppConfiguration _config;

        public WhoAmICommandHandler(IBotService botService, IBotAuthorizer botAuthorizer, AppConfiguration config)
        {
            _botService = botService ?? throw new ArgumentNullException(nameof(botService));
            _botAuthorizer = botAuthorizer ?? throw new ArgumentNullException(nameof(botAuthorizer));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string command) => command == Commands.WhoAmI;

        public async Task HandleCommand(IUpdateContext updateContext)
        {
            var user = _config.Users.FirstOrDefault(u => u.UserId == updateContext.User.UserId.ToString());
            var role = _botAuthorizer.GetUserType(updateContext.User.UserId);
            var message = "Tus datos:\n";

            if (user != null)
            {
                message += $" - Id: {user.UserId}\n" +
                           $" - Alias: {user.Alias}\n" +
                           $" - Nombre: {user.Name}\n";
            }
            else
            {
                message += " - No estas registrado\n";
            }
            message += $" - Rol: {role}";

            await _botService.SendTextMessageAsync(updateContext.User.UserId, message);
        }
    }
}
