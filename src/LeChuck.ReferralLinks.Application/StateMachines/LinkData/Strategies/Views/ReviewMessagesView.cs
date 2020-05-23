#region using directives

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.ReferralLinks.Application.Views;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Models;
using LeChuck.Telegram.Bot.Framework.Services;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData.Strategies.Views
{
    public class ReviewMessagesView : IMultiLinkStrategy
    {
        private readonly IBotService _bot;
        private readonly ILinkView _linkView;

        public ReviewMessagesView(IBotService bot, ILinkView linkView)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _linkView = linkView ?? throw new ArgumentNullException(nameof(linkView));
        }

        public bool CanHandle(string key) =>
            key == ProgramLinkStateMachineWorkflow.CommandsEnum.ReviewMessagesCmd.ToString()
            || key == ProgramLinkStateMachineWorkflow.CommandsEnum.ReviewOneMessage.ToString();

        public async Task<bool> Handle(IUpdateContext context, MultiLinkMessage entity,
            IStateMachine<IUpdateContext, MultiLinkMessage> stateMachine)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            var message = new StringBuilder();
            LinkMessage previewLinkMessage = null;
            var messageNumber = 0;

            var command = context.CallbackButtonData[0];
            if (command == ProgramLinkStateMachineWorkflow.CommandsEnum.ReviewOneMessage.ToString() &&
                int.TryParse(context.CallbackButtonData[1], out messageNumber))
            {
                previewLinkMessage = entity.Links.FirstOrDefault(l => l.Number == messageNumber);
                var viewData = _linkView.GetView(context.ChatId, previewLinkMessage);
                message.Append(viewData.Message);
            }
            else
            {
                message.Append("Selecciona un enlace para previsualizar");
            }

            var buttons = entity.Links.Where(link => link.Number != messageNumber).Select(link =>
                new BotButton(
                    $"Enlace {link.Number}",
                    ProgramLinkStateMachineWorkflow.CommandsEnum.ReviewOneMessage.ToString(),
                    link.Number.ToString())).ToList();
            buttons.Add(new BotButton("Volver", ProgramLinkStateMachineWorkflow.CommandsEnum.BackCmd.ToString()));

            if (previewLinkMessage == null)
                await _bot.SendTextMessageAsync(context.ChatId, message.ToString(), TextModeEnum.Html, buttons);
            else
                await _bot.SendPhotoAsync(context.ChatId, previewLinkMessage.PictureUrl, message.ToString(), TextModeEnum.Html,
                    buttons);

            return true;
        }
    }
}