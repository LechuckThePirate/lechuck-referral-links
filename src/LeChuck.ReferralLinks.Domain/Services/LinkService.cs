using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Enums;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Domain.Services
{
    public class LinkService : ILinkService
    {
        private readonly ILogger<LinkService> _logger;
        private readonly IHtmlParserProvider _htmlParserProvider;
        private readonly IUrlShortenerProvider _urlShortenerProvider;

        public LinkService(ILogger<LinkService> logger,
            IHtmlParserProvider htmlParserProvider,
            IUrlShortenerProvider urlShortenerProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _htmlParserProvider = htmlParserProvider ?? throw new ArgumentNullException(nameof(htmlParserProvider));
            _urlShortenerProvider = urlShortenerProvider ?? throw new ArgumentNullException(nameof(urlShortenerProvider));
        }

        public async Task<LinkData> BuildMessage(string url)
        {
            var shortener = _urlShortenerProvider.GetServiceOrDefault(UrlShortenersEnum.None);
            var parser = _htmlParserProvider.GetParserFor(url);
            if (parser == null)
            {
                _logger.LogWarning($"No parser for url: {url}");
                return null;
            }

            var message = parser.ParseUrl(url);
            var shortUrl = shortener.ShortenUrl(url);

            Task.WaitAll(message, shortUrl);

            if (await message == null || await shortUrl == null)
                return null;

            message.Result.ShortenedUrl = shortUrl.Result;

            return await message;
        }
    }
}
