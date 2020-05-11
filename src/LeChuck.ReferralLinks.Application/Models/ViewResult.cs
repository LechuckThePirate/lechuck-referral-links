using System;
using System.Collections.Generic;
using System.Text;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Models;

namespace LeChuck.ReferralLinks.Application.Models
{
    public class ViewResult
    {
        public string Message { get; set; }
        public TextModeEnum ParseMode { get; set; }
        public List<BotButton> Buttons { get; set; }
    }
}
