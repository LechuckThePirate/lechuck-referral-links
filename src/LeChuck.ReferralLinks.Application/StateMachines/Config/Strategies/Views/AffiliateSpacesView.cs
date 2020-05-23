using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Providers;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Models;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.StateMachines.Config.Strategies.Views
{
    public class AffiliateSpacesView : IConfigStrategy
    {
        private readonly ILogger<AffiliateSpacesView> _logger;
        private readonly IAffiliateProvider _affiliateProvider;
        private readonly IBotService _bot;

        public AffiliateSpacesView(ILogger<AffiliateSpacesView> logger, IAffiliateProvider affiliateProvider, IBotService bot)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _affiliateProvider = affiliateProvider ?? throw new ArgumentNullException(nameof(affiliateProvider));
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
        }

        public bool CanHandle(string key) =>
            key == ConfigStateMachineWorkflow.StatesEnum.SelectAffiliateSpaceState.ToString();

        public async Task<bool> Handle(IUpdateContext context, AppConfiguration entity, IStateMachine<IUpdateContext, AppConfiguration> stateMachine)
        {
            var selectedAffiliate =
                stateMachine.GetParameter<AffiliateConfig>(ConfigStateMachineWorkflow.Params.SelectedAffiliate);
            if (selectedAffiliate == null)
            {
                _logger.LogError("No affiliate selected!");
                return false;
            }

            var affiliate = _affiliateProvider.GetAffiliateByName(selectedAffiliate.Name);
            if (affiliate == null)
            {
                _logger.LogError("Affiliate not found");
                return false;
            }
            var spaces = await affiliate.GetSpaces();
            var message = GetMessage();
            var buttons = GetButtons(spaces);

            await _bot.SendTextMessageAsync(context.User.UserId, message.ToString(), TextModeEnum.Html,
                buttons);

            return true;

        }

        private List<BotButton> GetButtons(IEnumerable<AffiliateSpace> spaces)
        {
            var buttons = new List<BotButton>();
            buttons.AddRange(spaces.Select(e =>
                new BotButton($"{e.Name} ({e.Id})",
                    ConfigStateMachineWorkflow.CommandsEnum.SetAffiliateSpaceCmd.ToString(),
                    e.Id.ToString())
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
