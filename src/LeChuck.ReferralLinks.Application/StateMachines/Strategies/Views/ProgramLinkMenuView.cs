using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.ProgramLink;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Models;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.StateMachines.Strategies.Views
{
    public class ProgramLinkMenuView : ILinkDataStrategy
    {
        private readonly IBotService _bot;
        public ProgramLinkMenuView(IBotService bot)
        {
            _bot = bot;
        }

        public bool CanHandle(string key) => key == ProgramLinkStateMachineWorkflow.StatesEnum.MenuState.ToString();

        public async Task<bool> Handle(IUpdateContext context, LinkData entity)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            var message = new StringBuilder("Enviar/Programar enlace:\n\n");
            message.Append($"<b>Titulo:</b> {entity.Title}\n");
            message.Append($"<b>Url Imagen:</b> {entity.PictureUrl}\n");
            message.Append($"<b>Pvp:</b> {entity.OriginalPrice}\n");
            message.Append($"<b>Precio final:</b> {entity.FinalPrice}\n");
            message.Append($"<b>Ahorro</b> {entity.SavedPrice}\n");
            message.Append($"<b>Url:</b> {entity.ShortenedUrl}\n");
            message.Append($"<b>Canales:</b> {string.Join(",", entity.Channels.Select(c =>c.ChannelName))}\n");

            var buttons = new List<BotButton>
            {
                new BotButton("Cambiar Enlace", $"{ProgramLinkStateMachineWorkflow.CommandsEnum.SetUrlCmd}"),
                new BotButton("Canales", $"{ProgramLinkStateMachineWorkflow.CommandsEnum.SelectChannelsCmd}"),
                new BotButton("Programar", $"{ProgramLinkStateMachineWorkflow.CommandsEnum.SelectTimeSpanCmd}"),
                new BotButton("Enviar", $"{ProgramLinkStateMachineWorkflow.CommandsEnum.SaveCmd}"),
                new BotButton("Cancelar", $"{ProgramLinkStateMachineWorkflow.CommandsEnum.CancelCmd}")
            };

            await _bot.SendTextMessageAsync(context.UserId, message.ToString(), TextModeEnum.Html,
                buttons);
            return true;
        }
    }
}
