using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeChuck.ReferralLinks.Domain.Models;

namespace LeChuck.ReferralLinks.Domain.Services
{
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
