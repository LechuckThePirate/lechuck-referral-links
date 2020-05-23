#region using directives

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Models;
using LeChuck.Telegram.Bot.Framework.Services;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData.Strategies.Views
{
    public class MenuView : IMultiLinkStrategy
    {
        private readonly IBotService _bot;

        public MenuView(IBotService bot)
        {
            _bot = bot;
        }

        public bool CanHandle(string key) => key == ProgramLinkStateMachineWorkflow.StatesEnum.MenuState.ToString();

        public async Task<bool> Handle(IUpdateContext context, MultiLinkMessage entity,
            IStateMachine<IUpdateContext, MultiLinkMessage> stateMachine)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            var numLinks = entity.Links.Count;
            var numChannels = !entity.Channels.Any() ? " todos los canales" : $" {entity.Channels.Count} canales:";

            var message = new StringBuilder("Enviar/Programar enlaces:\n\n");
            message.Append($"  <b>{numLinks} enlaces en el paquete:</b>\n");
            message.Append($"  - {string.Join("\n  - ", entity.Links.Select(e => e.Url))}\n");
            message.Append($"\n  <b>Enviar a{numChannels}</b>\n");

            if (entity.Channels.Any())
                message.Append($"  - {string.Join("\n  - ", entity.Channels.Select(c => c.ChannelName))}\n");

            var channelCount = entity.Channels.Any()
                ? entity.Channels.Count.ToString()
                : "Todos";

            var buttons = new List<BotButton>
            {
                new BotButton("Cambiar Enlace", $"{ProgramLinkStateMachineWorkflow.CommandsEnum.SetUrlCmd}"),
                new BotButton($"Canales ({channelCount})",
                    $"{ProgramLinkStateMachineWorkflow.CommandsEnum.SelectChannelsCmd}"),
                new BotButton("Programar", $"{ProgramLinkStateMachineWorkflow.CommandsEnum.SelectTimeSpanCmd}"),
                new BotButton("Ver/Revisar", $"{ProgramLinkStateMachineWorkflow.CommandsEnum.ReviewMessagesCmd}"),
                new BotButton("Enviar", $"{ProgramLinkStateMachineWorkflow.CommandsEnum.SendCmd}"),
                new BotButton("Cancelar", $"{ProgramLinkStateMachineWorkflow.CommandsEnum.CancelCmd}")
            };

            await _bot.SendTextMessageAsync(context.User.UserId, message.ToString(), TextModeEnum.Html,
                buttons, webPreview: false);
            return true;
        }
    }
}