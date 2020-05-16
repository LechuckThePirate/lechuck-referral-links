#region using directives

using System;
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
    public class SelectChannelsStateView : IMultiLinkStrategy
    {
        private readonly IBotService _bot;
        private readonly AppConfiguration _configuration;

        public SelectChannelsStateView(IBotService botService, AppConfiguration configuration)
        {
            _bot = botService ?? throw new ArgumentNullException(nameof(botService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public bool CanHandle(string key) =>
            key == ProgramLinkStateMachineWorkflow.StatesEnum.SelectChannelsState.ToString();

        public async Task<bool> Handle(IUpdateContext context, MultiLink entity,
            IStateMachine<IUpdateContext, MultiLink> stateMachine)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            var selectedChannels = entity.Channels;
            var availableChannels =
                _configuration.Channels.Where(c => selectedChannels.All(s => s.ChannelId != c.ChannelId));
            var message = new StringBuilder();
            message.Append("Canales Seleccionados:\n\n");
            foreach (var channel in selectedChannels)
                message.Append($" - {channel.ChannelName}\n");
            var buttons = new List<BotButton>();
            if (availableChannels.Any())
            {
                message.Append("\nSeleccionar canales a añadir o Volver");
                foreach (var channel in availableChannels)
                {
                    buttons.Add(
                        new BotButton(channel.ChannelName,
                            ProgramLinkStateMachineWorkflow.CommandsEnum.AddChannelCmd.ToString(),
                            channel.ChannelId.ToString())
                    );
                }
            }
            else
            {
                message.Append("\nNo quedan canales por seleccionar");
            }

            if (selectedChannels.Any())
            {
                buttons.Add(new BotButton("Reiniciar",
                    ProgramLinkStateMachineWorkflow.CommandsEnum.ResetChanels.ToString()));
            }

            buttons.Add(new BotButton("Volver", ProgramLinkStateMachineWorkflow.CommandsEnum.BackCmd.ToString()));

            await _bot.SendTextMessageAsync(context.User.UserId, message.ToString(), TextModeEnum.Html,
                buttons);

            return true;
        }
    }
}