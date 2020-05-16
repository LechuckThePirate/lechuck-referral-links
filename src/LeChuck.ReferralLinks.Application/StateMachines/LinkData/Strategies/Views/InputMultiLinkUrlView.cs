#region using directives

using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData.Strategies.Views
{
    public class InputMultiLinkUrlView : IMultiLinkStrategy
    {
        private readonly IBotService _bot;

        public InputMultiLinkUrlView(IBotService bot)
        {
            _bot = bot;
        }

        public bool CanHandle(string key) => key == ProgramLinkStateMachineWorkflow.StatesEnum.InputUrlState.ToString();

        public async Task<bool> Handle(IUpdateContext context, MultiLink entity,
            IStateMachine<IUpdateContext, MultiLink> stateMachine)
        {
            await _bot.SendTextMessageAsync(context.ChatId, "Introduce la url");
            return true;
        }
    }
}