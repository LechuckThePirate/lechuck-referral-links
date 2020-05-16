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
    public class AffiliatesView : IConfigStrategy
    {
        private readonly IBotService _bot;

        public AffiliatesView(IBotService bot)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) => key == ConfigStateMachineWorkflow.StatesEnum.AffiliatesState.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity,
            IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            var message = new StringBuilder("<b>AFILIACIONES</b>\n\nAfiliaciones activas:\n\n");
            message.Append("  - " +
                           $"{string.Join("\n  - ", entity.AffiliateServices.Select(s => s.Service))}\n");
            message.Append("\nSelecciona una opción");
            var buttons = new List<BotButton>
            {
                new BotButton("Añadir", ConfigStateMachineWorkflow.CommandsEnum.AddAffiliateCmd.ToString()),
                new BotButton("Borrar", ConfigStateMachineWorkflow.CommandsEnum.RemoveAffiliateCmd.ToString()),
                new BotButton("Configurar", ConfigStateMachineWorkflow.CommandsEnum.ConfigureAffiliateCmd.ToString()),
                new BotButton("Atrás", ConfigStateMachineWorkflow.CommandsEnum.BackCmd.ToString())
            };

            await _bot.SendTextMessageAsync(context.User.UserId, message.ToString(), TextModeEnum.Html,
                buttons);
            return true;
        }
    }
}