using System;
using System.Linq;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application.StateMachines.ProgramLink;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Services;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;

namespace LeChuck.ReferralLinks.Application.StateMachines.Strategies.Commands
{
    public class SetUrlCommandStrategy : ILinkDataStrategy
    {
        private readonly IBotService _bot;
        private readonly IHtmlParserProvider _htmlParserProvider;
        private readonly ILinkService _linkService;

        public SetUrlCommandStrategy(IBotService bot, IHtmlParserProvider htmlParserProvider, ILinkService linkService)
        {
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _htmlParserProvider = htmlParserProvider ?? throw new ArgumentNullException(nameof(htmlParserProvider));
            _linkService = linkService ?? throw new ArgumentNullException(nameof(linkService));
        }

        public bool CanHandle(string key) => key == ProgramLinkStateMachineWorkflow.CommandsEnum.SetUrlCmd.ToString();

        public async Task<bool> Handle(IUpdateContext context, LinkData entity)
        {
            var content = context.Content.FirstOrDefault(c => c.Type == "Url");
            if (content == null)
            {
                await _bot.SendTextMessageAsync(context.ChatId, "No has introducido un enlace");
                return false;
            }

            var url = content.Value.ToLowerInvariant().StartsWith("http")
                ? content.Value
                : $"https://{content.Value}";

            if (_htmlParserProvider.GetParserFor(url) == null)
            {
                await _bot.SendTextMessageAsync(context.ChatId, "Enlace no soportado");
                return false;
            }

            var message = await _linkService.BuildMessage(url);
            entity.Title = message.Title;
            entity.FinalPrice = message.FinalPrice;
            entity.LongUrl = message.LongUrl;
            entity.ShortenedUrl = message.ShortenedUrl;
            entity.OriginalPrice = message.OriginalPrice;
            entity.SavedPrice = message.SavedPrice;
            entity.PictureUrl = message.PictureUrl;
            
            return true;
        }
    }
}
