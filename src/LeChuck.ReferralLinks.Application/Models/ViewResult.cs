#region using directives

using System.Collections.Generic;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Models;

#endregion

namespace LeChuck.ReferralLinks.Application.Models
{
    public class ViewResult
    {
        public string Message { get; set; }
        public TextModeEnum ParseMode { get; set; }
        public List<BotButton> Buttons { get; set; }
    }
}