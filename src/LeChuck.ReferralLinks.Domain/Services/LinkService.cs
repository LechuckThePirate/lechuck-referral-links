using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Enums;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Providers;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Domain.Services
{
    public class LinkService : ILinkService
    {
        private readonly ILogger<LinkService> _logger;
        private readonly IAffiliateProvider _affiliateProvider;
        private readonly ILinkParserProvider _linkParserProvider;
        private readonly IUrlShortenerProvider _urlShortenerProvider;

        public LinkService(ILogger<LinkService> logger,
            IAffiliateProvider affiliateProvider,
            ILinkParserProvider linkParserProvider,
            IUrlShortenerProvider urlShortenerProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _affiliateProvider = affiliateProvider ?? throw new ArgumentNullException(nameof(affiliateProvider));
            _linkParserProvider = linkParserProvider ?? throw new ArgumentNullException(nameof(linkParserProvider));
            _urlShortenerProvider = urlShortenerProvider ?? throw new ArgumentNullException(nameof(urlShortenerProvider));
        }

        public async Task<Link> BuildMessage(string url)
        {
            var affiliate = _affiliateProvider.GetAffiliateFor(url);
            if (affiliate != null)
            {
                url = await affiliate.GetCommisionedDeepLink(url);
            }
            else
            {
                _logger.LogWarning($"No affiliate for url: {url}");
            }

            var parser = _linkParserProvider.GetParserFor(url);
            if (parser == null)
            {
                _logger.LogWarning($"No parser for url: {url}");
                return null;
            }

            var message = parser.ParseUrl(url);
            var shortener = _urlShortenerProvider.GetServiceOrDefault(UrlShortenersEnum.BitLy);
            var shortUrl = shortener.ShortenUrl(url);

            Task.WaitAll(message, shortUrl);

            if (await message == null || await shortUrl == null)
                return null;

            message.Result.ShortenedUrl = shortUrl.Result;

            return await message;
        }
    }
}
