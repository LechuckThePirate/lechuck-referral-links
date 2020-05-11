using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData.Strategies.Views
{
    public class CancelView : IMultiLinkStrategy
    {
        private readonly IBotService _bot;
        public CancelView(IBotService bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) => key == ProgramLinkStateMachineWorkflow.StatesEnum.CancelledState.ToString();

        public async Task<bool> Handle(IUpdateContext context, Domain.Models.MultiLink entity)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            await _bot.SendTextMessageAsync(context.ChatId, "Comando cancelado.");
            return true;
        }
    }
}
