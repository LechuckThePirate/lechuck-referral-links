using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Centvrio.Emoji;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Models;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Views
{
    public class SelectedVendorView : IConfigStrategy
    {
        private readonly ILogger<SelectedVendorView> _logger;
        private readonly IBotService _bot;

        public SelectedVendorView(ILogger<SelectedVendorView> logger, IBotService bot)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) =>
            key == ConfigStateMachineWorkflow.StatesEnum.SelectedVendorState.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            var vendor =
                stateMachine.GetParameter<VendorConfig>(ConfigStateMachineWorkflow.Params.SelectedVendor);
            if (vendor == null)
            {
                _logger.LogError("No vendor selected");
                return false;
            }

            var message = GetMessage(vendor);
            var buttons = GetButtons();

            await _bot.SendTextMessageAsync(context.ChatId, message.ToString(), TextModeEnum.Html, buttons);
            return true;
        }

        private static List<BotButton> GetButtons()
        {
            var buttons = new List<BotButton>
            {
                new BotButton("GotoLink",
                    ConfigStateMachineWorkflow.CommandsEnum.InputVendorGotoLinkCmd.ToString()),
                new BotButton("Atrás", ConfigStateMachineWorkflow.CommandsEnum.BackCmd.ToString())
            };
            return buttons;
        }

        private static StringBuilder GetMessage(VendorConfig vendor)
        {
            var message = new StringBuilder();
            message.AppendLine($"<b>{vendor.Name}</b>");
            message.AppendLine(!string.IsNullOrWhiteSpace(vendor.AffiliateCustomizer)
                ? $"  - {vendor.CustomizerPrompt}: {vendor.AffiliateCustomizer}"
                : $"  - {vendor.CustomizerPrompt}: {TransportGround.StopSign} <b>No introducido</b>");

            message.AppendLine();
            return message;
        }
    }
}
