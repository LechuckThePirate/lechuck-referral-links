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

}
