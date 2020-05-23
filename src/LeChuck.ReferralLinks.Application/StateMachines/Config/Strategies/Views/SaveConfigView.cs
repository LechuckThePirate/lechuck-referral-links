using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Views
{
    public class SaveConfigView : IConfigStrategy
    {
        private readonly IBotService _bot;

        public SaveConfigView(IBotService bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) => key == ConfigStateMachineWorkflow.StatesEnum.DoneState.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            await _bot.SendTextMessageAsync(context.ChatId, "Se ha guardado la configuración.");
            return true;
        }
    }
}
