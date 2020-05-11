using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

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

        public async Task<bool> Handle(IUpdateContext context, Domain.Models.MultiLink entity)
        {
            await _bot.SendTextMessageAsync(context.ChatId, "Introduce la url");
            return true;
        }
    }
}
