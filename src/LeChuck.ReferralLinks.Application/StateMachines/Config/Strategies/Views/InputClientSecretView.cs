using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Views
{
    public class InputClientSecretView : IConfigStrategy
    {
        private readonly IBotService _bot;

        public InputClientSecretView(IBotService bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) => key == ConfigStateMachineWorkflow.StatesEnum.InputClientSecretState.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            await _bot.SendTextMessageAsync(context.ChatId, "Introduce tu client_secret");
            return true;
        }
    }
}
