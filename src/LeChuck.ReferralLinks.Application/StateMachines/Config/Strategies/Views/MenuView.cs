#region using directives

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Models;
using LeChuck.Telegram.Bot.Framework.Services;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Views
{
    public class MenuView : IConfigStrategy
    {
        private readonly IBotService _bot;

        public MenuView(IBotService bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) => key == ConfigStateMachineWorkflow.StatesEnum.HomeState.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity,
            IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            var message = "<b>CONFIGURACION</b>\n\nSelecciona una opción:";
            var buttons = new List<BotButton>
            {
                new BotButton("Afiliados", ConfigStateMachineWorkflow.CommandsEnum.AffiliatesCmd.ToString()),
                new BotButton("Cancelar", ConfigStateMachineWorkflow.CommandsEnum.CancelConfigCmd.ToString()),
                new BotButton("Guardar", ConfigStateMachineWorkflow.CommandsEnum.SaveConfigCmd.ToString())
            };

            await _bot.SendTextMessageAsync(context.User.UserId, message, TextModeEnum.Html,
                buttons);
            return true;
        }
    }
}