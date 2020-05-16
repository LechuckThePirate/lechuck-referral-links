#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.UnitsOfWork;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.Extensions.Logging;

#endregion

namespace LeChuck.ReferralLinks.Application.CommandHandlers
{
    public class RegisterUserCommandHandler : ICommandHandler
    {
        private readonly IBotService _bot;
        private readonly IConfigUnitOfWork _configUnitOfWork;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly AppConfiguration _config;

        public RegisterUserCommandHandler(
            IBotService bot,
            IConfigUnitOfWork configUnitOfWork,
            ILogger<RegisterUserCommandHandler> logger,
            AppConfiguration config)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _configUnitOfWork = configUnitOfWork ?? throw new ArgumentNullException(nameof(configUnitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string command) => command == Commands.RegisterUser;

        public async Task HandleCommand(IUpdateContext updateContext)
        {
            if (updateContext.ForwardedUser == null)
            {
                await _bot.SendTextMessageAsync(updateContext.User.UserId,
                    "Debes reenviar un mensaje del usuario a añadir para poder obtener su identidad.");
                return;
            }

            var newUser = new User
            {
                UserId = updateContext.ForwardedUser.UserId.ToString(),
                Alias = updateContext.ForwardedUser.Alias,
                Name = updateContext.ForwardedUser.Name
            };

            if (_config.Users.All(u => u.UserId != newUser.UserId))
            {
                _config.Users.Add(newUser);

                await _configUnitOfWork.SaveConfig(_config);
                _logger.LogInformation($"User {newUser} added to bot admins");
                await _bot.SendTextMessageAsync(updateContext.User.UserId, $"Usuario {newUser} añadido como admin");
                return;
            }

            _logger.LogWarning($"The user {newUser} was already an admin!");
        }
    }
}