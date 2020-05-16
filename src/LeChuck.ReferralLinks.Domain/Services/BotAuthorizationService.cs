#region using directives

using System;
using System.Linq;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Enums;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services
{
    public class BotAuthorizationService : IAuthorizationService
    {
        private readonly AppConfiguration _config;

        public BotAuthorizationService(AppConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool IsRoot(long userId)
        {
            return userId.ToString() == _config.RootUserId;
        }

        public UserTypeEnum GetUserType(long userId)
        {
            if (IsRoot(userId)) return UserTypeEnum.Root;
            if (IsAdmin(userId)) return UserTypeEnum.Admin;
            return UserTypeEnum.RegularUser;
        }

        public bool IsAdmin(long userId)
        {
            return IsRoot(userId) || (_config.Users?.Any(u => u.UserId == userId.ToString()) ?? false);
        }
    }
}