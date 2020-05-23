#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class SelectAffiliatesView : IConfigStrategy
    {
        private readonly IBotService _bot;
        private readonly AppConfiguration _config;

        public SelectAffiliatesView(IBotService bot, AppConfiguration config)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool CanHandle(string key) => key == ConfigStateMachineWorkflow.StatesEnum.AffiliatesState.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity,
            IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            var message = GetMessage();
            var buttons = GetButtons();

            await _bot.SendTextMessageAsync(context.User.UserId, message.ToString(), TextModeEnum.Html,
                buttons);
            return true;
        }

        private List<BotButton> GetButtons()
        {
            var buttons = new List<BotButton>();
            buttons.AddRange(_config.AffiliateServices.Select(aff =>
                new BotButton($"{aff.Name} ({(aff.Enabled ? "activo" : "inactivo")})",
                    ConfigStateMachineWorkflow.CommandsEnum.SelectAffiliateCmd.ToString(),
                    aff.Name)
            ));
            buttons.Add(new BotButton("Atrás", ConfigStateMachineWorkflow.CommandsEnum.BackCmd.ToString()));
            return buttons;
        }

        private static StringBuilder GetMessage()
        {
            var message = new StringBuilder();
            message.AppendLine("<b>AFILIACIONES</b>");
            message.AppendLine();
            message.Append("Selecciona una afiliación para configurarla");
            return message;
        }
    }
}