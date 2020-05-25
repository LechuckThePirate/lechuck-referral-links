#region using directives

using System;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Services;
using LeChuck.Stateless.StateMachine;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

#endregion

namespace LeChuck.ReferralLinks.Application.StateMachines.LinkData.Strategies.Commands
{
    public class SetUrlCommandStrategy : IMultiLinkStrategy
    {
        private readonly IBotService _bot;
        private readonly IVendorProvider _vendorProvider;

        public SetUrlCommandStrategy(IBotService bot, IVendorProvider vendorProvider)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _vendorProvider = vendorProvider ?? throw new ArgumentNullException(nameof(vendorProvider));
        }

        public bool CanHandle(string key) => key == ProgramLinkStateMachineWorkflow.CommandsEnum.SetUrlCmd.ToString();

        public async Task<bool> Handle(IUpdateContext context, MultiLinkMessage entity,
            IStateMachine<IUpdateContext, MultiLinkMessage> stateMachine)
        {
            var content = context.Content.FirstOrDefault(c => c.Type == "Url");
            if (content == null)
            {
                await _bot.SendTextMessageAsync(context.ChatId,
                    "No has introducido un enlace. Introduce un enlace soportado o /cancelar");
                return false;
            }

            var url = content.Value.ToLowerInvariant().StartsWith("http")
                ? content.Value
                : $"https://{content.Value}";

            if (_vendorProvider.GetVendorFor(url) == null)
            {
                await _bot.SendTextMessageAsync(context.ChatId, "Enlace no soportado");
                return false;
            }

            throw new NotImplementedException();

            //var message = await _linkService.BuildMessage(url);
            // TODO: Refactor
            //entity.Title = message.Title;
            //entity.FinalPrice = message.FinalPrice;
            //entity.LongUrl = message.LongUrl;
            //entity.ShortenedUrl = message.ShortenedUrl;
            //entity.OriginalPrice = message.OriginalPrice;
            //entity.SavedPrice = message.SavedPrice;
            //entity.PictureUrl = message.PictureUrl;

        }
    }
}