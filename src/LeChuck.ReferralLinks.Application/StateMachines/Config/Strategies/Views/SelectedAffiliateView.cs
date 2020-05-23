using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Centvrio.Emoji;
using LeChuck.ReferralLinks.Application.Extensions;
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
    public class SelectedAffiliateView : IConfigStrategy
    {
        private readonly ILogger<SelectedAffiliateView> _logger;
        private readonly AppConfiguration _config;
        private readonly IBotService _bot;

        public SelectedAffiliateView(ILogger<SelectedAffiliateView> logger, AppConfiguration config, IBotService bot)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) =>
            key == ConfigStateMachineWorkflow.StatesEnum.SelectedtAffiliateState.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            if (context.CallbackMessageId.HasValue)
                await _bot.DeleteMessageAsync(context.ChatId, context.CallbackMessageId.Value);

            var affiliate =
                stateMachine.GetParameter<AffiliateConfig>(ConfigStateMachineWorkflow.Params.SelectedAffiliate);
            if (affiliate == null)
            {
                _logger.LogError("No affiliate selected");
                return false;
            }

            var message = GetMessage(affiliate);
            var buttons = GetButtons(affiliate);

            await _bot.SendTextMessageAsync(context.ChatId, message.ToString(), TextModeEnum.Html, buttons);
            return true;
        }

        private static List<BotButton> GetButtons(AffiliateConfig affiliate)
        {
            var buttons = new List<BotButton>
            {
                new BotButton("Credenciales",
                    ConfigStateMachineWorkflow.CommandsEnum.SetAffiliateCredentialsCmd.ToString()),
                new BotButton("Espacio",
                    ConfigStateMachineWorkflow.CommandsEnum.SelectAffiliateSpaceCmd.ToString()),
                new BotButton(affiliate.Enabled ? "Desactivar" : "Activar",
                    ConfigStateMachineWorkflow.CommandsEnum.ToggleActiveAffiliateCmd.ToString()),
                new BotButton("Atrás", ConfigStateMachineWorkflow.CommandsEnum.BackCmd.ToString())
            };
            return buttons;
        }

        private static StringBuilder GetMessage(AffiliateConfig affiliate)
        {
            var message = new StringBuilder();
            message.AppendLine($"<b>{affiliate.Name}</b>");
            message.AppendLine($"  - Endpoint: {affiliate.ApiEndpoint}");
            if (affiliate.IsValidCredentials())
            {
                message.AppendLine($"  - Credenciales:");
                message.AppendLine($"     - Id. Cliente: {affiliate.Credentials.ClientId.SecurizeString()}");
                message.AppendLine($"     - Secreto: {affiliate.Credentials.ClientSecret.SecurizeString()}");
            }
            else
            {
                message.AppendLine($"  - Credenciales: {TransportGround.StopSign} <b>No hay credenciales</b>");
            }

            message.AppendLine(affiliate.IsValidSpace()
                ? $"  - Espacio: {affiliate.SpaceId}"
                : $"  - Espacio: {TransportGround.StopSign} <b>No seleccionado</b>");

            message.AppendLine();
            message.AppendLine($"  - <b>Estado: {(affiliate.Enabled ? "activo" : $"{TransportGround.StopSign} inactivo")}</b>");
            return message;
        }
    }
}
