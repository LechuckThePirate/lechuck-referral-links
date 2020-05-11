using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Models;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Views
{
    public class MenuView : IConfigStrategy
    {
        private readonly IBotService _bot;

        public MenuView(IBotService bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) => key == ConfigStateMachineWorkflow.StatesEnum.MenuState.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity)
        {
            var message = "Selecciona una opción:";
            var buttons = new List<BotButton>
            {
                new BotButton("Canales", $"{ConfigStateMachineWorkflow.CommandsEnum.ChannelsCmd}"),
                new BotButton("Acortar Enlaces",$"{ConfigStateMachineWorkflow.CommandsEnum.DefaultShortenerCmd}"),
                new BotButton("Guardar config",$"{ConfigStateMachineWorkflow.CommandsEnum.SaveCmd}"),
                new BotButton("Cancelar", $"{ConfigStateMachineWorkflow.CommandsEnum.CancelCmd}")
            };

            await _bot.SendTextMessageAsync(context.User.UserId, message.ToString(), TextModeEnum.Html,
                buttons);
            return true;

        }
    }
}
