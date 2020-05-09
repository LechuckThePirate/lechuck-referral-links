using System;
using System.Linq;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Interfaces;

namespace LeChuck.ReferralLinks.Domain.Services
{
    public interface IAuthorizationService : IBotAuthorizer
    {
        bool IsRootUser(long userId);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly AppConfiguration _config;

        public AuthorizationService(AppConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool IsRootUser(long userId)
        {
            return userId.ToString() == _config.RootUserId;
        }

        public bool IsAdmin(long userId)
        {
            return IsRootUser(userId) || (_config.Users?.Any(u => u.UserId == userId.ToString()) ?? false);
        }
    }
}
