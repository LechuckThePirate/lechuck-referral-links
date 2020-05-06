using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Centvrio.Emoji;
using LeChuck.ReferralLinks.Domain;
using LeChuck.ReferralLinks.Domain.Enums;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.Telegram.Bot.Framework.Enums;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using LeChuck.Telegram.Bot.Framework.Services;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Application.UpdateHandlers
{
    public class LinkUpdateHandler : IUpdateHandler
    {
        private readonly ILogger<LinkUpdateHandler> _logger;
        private readonly IBotService _bot;
        private readonly IUrlShortenerProvider _urlShortenerProvider;
        private readonly IHtmlParserProvider _htmlParserProvider;

        public LinkUpdateHandler(ILogger<LinkUpdateHandler> logger, IBotService bot, IUrlShortenerProvider urlShortenerProvider, IHtmlParserProvider htmlParserProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bot = bot ?? throw new ArgumentNullException(nameof(bot));
            _urlShortenerProvider = urlShortenerProvider ?? throw new ArgumentNullException(nameof(urlShortenerProvider));
            _htmlParserProvider = htmlParserProvider ?? throw new ArgumentNullException(nameof(htmlParserProvider));
        }

        public bool CanHandle(IUpdateContext update) => update.Content.Any(c => c.Type == Constants.MessageContentType.Url);

        public async Task HandleUpdate(IUpdateContext updateContext)
        {
            _logger.LogTrace($"Handling update: {updateContext}");

            var url = updateContext.Content.FirstOrDefault(c => c.Type == Constants.MessageContentType.Url)?.Value;
            if (string.IsNullOrEmpty(url))
                return;

            var parser = _htmlParserProvider.GetParserFor(url);
            if (parser == null)
            {
                _logger.LogWarning($"No parser for url: {url}");
                return;
            }

            // Run calls in parallel
            var message = parser.ParseUrl(url);
            var shortener = _urlShortenerProvider.GetServiceOrDefault(UrlShortenersEnum.BitLy);
            var shortUrl = shortener.ShortenUrl(url);

            Task.WaitAll(message, shortUrl);

            if (message.Result == null || shortUrl.Result == null)
                return;

            message.Result.ShortenedUrl = shortUrl?.Result;

            await _bot.DeleteMessageAsync(updateContext.ChatId, updateContext.MessageId ?? 0);
            await _bot.SendPhotoAsync(updateContext.ChatId, message.Result.PictureUrl, 
                $"\n{Event.Ribbon} <b>{message.Result.Title}</b>\n" +
                $"\n" +
                $"{Money.Euro} <b>PRECIO:</b> {message.Result.Price}\n" +
                $"\n" +
                $"{HouseHold.ShoppingCart} {message.Result.ShortenedUrl}", TextModeEnum.Html);
        }
    }
}
