#region using directives

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Centvrio.Emoji;
using LeChuck.ReferralLinks.Application.Models;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Models;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.Extensions.Logging;

#endregion

namespace LeChuck.ReferralLinks.Application.Views
{
    public interface ILinkView : IView<Link>
    {
        ViewResult GetView(long chatId, Link data);
    }

    public class LinkView : ILinkView
    {
        private readonly ILogger<LinkView> _logger;
        private readonly IBotService _bot;

        public LinkView(ILogger<LinkView> logger, IBotService bot)
        {
            _logger = logger;
            _bot = bot;
        }

        public async Task SendView(long chatId, Link data)
        {
            var viewData = GetView(chatId, data);
            await _bot.SendPhotoAsync(chatId, data.PictureUrl, viewData.Message, viewData.ParseMode);
            ;
        }

        public ViewResult GetView(long chatId, Link data)
        {
            var message = new StringBuilder();
            message.Append($"\n{Event.Ribbon} <b>{data.Title}</b>\n\n");
            if (!string.IsNullOrWhiteSpace(data.OriginalPrice))
                message.Append($"{OtherSymbols.CrossMark} PVP: {data.OriginalPrice}\n");
            message.Append($"{Money.Euro} <b>PRECIO FINAL: {data.FinalPrice}</b>\n");
            if (!string.IsNullOrWhiteSpace(data.SavedPrice))
                message.Append($"{Clothing.Purse} Ahorras {data.SavedPrice}\n");
            message.Append($"\n{HouseHold.ShoppingCart} {data.ShortenedUrl}");

            return new ViewResult
            {
                Message = message.ToString(),
                Buttons = new List<BotButton>(),
                ParseMode = TextModeEnum.Html
            };
        }
    }
}