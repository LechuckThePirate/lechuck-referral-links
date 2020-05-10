using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.ProgramLink;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.StateMachines.Strategies.Views
{
    public class InputLinkUrlView : ILinkDataStrategy
    {
        private readonly IBotService _bot;

        public InputLinkUrlView(IBotService bot)
        {
            _bot = bot;
        }

        public bool CanHandle(string key) => key == ProgramLinkStateMachineWorkflow.StatesEnum.InputUrlState.ToString();

        public async Task<bool> Handle(IUpdateContext context, LinkData entity)
        {
            await _bot.SendTextMessageAsync(context.ChatId, "Introduce la url");
            return true;
        }
    }
}
