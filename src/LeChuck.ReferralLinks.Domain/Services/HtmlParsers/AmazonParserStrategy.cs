#region using directives

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using LeChuck.ReferralLinks.Domain.Extensions;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;
using Microsoft.Extensions.Logging;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services.HtmlParsers
{
    public class AmazonParserStrategy : ILinkParserStrategy
    {
        private readonly ILogger<AmazonParserStrategy> _logger;

        private static readonly Regex TitleRegex =
            new Regex("<h1 id=\"title\".+?(?=productTitle).+?(?=>)>(.+?(?=<\\/span>))", RegexOptions.Singleline);

        private static readonly Regex PictureRegex =
            new Regex("<div id=\"imgTagWrapperId\".+?(?=https\\:\\/\\/)(.+?(?=\"))", RegexOptions.Singleline);

        private static readonly Regex PriceRegex =
            new Regex("<span id=\"priceblock_(ourprice|dealprice)\".+?(?=>)>(.+?(?=<))");

        private static readonly Regex OriginalPriceRegex =
            new Regex("<span class=\"priceBlockStrikePriceString.+?(?=>)>(.+?(?=<))", RegexOptions.Singleline);

        private static readonly Regex PriceSavesRegex =
            new Regex("priceBlockSavingsString\">(.+?(?=<))", RegexOptions.Singleline);

        private VendorConfig _config;
        private IUrlShortenerStrategy _shortener;

        public AmazonParserStrategy(ILogger<AmazonParserStrategy> logger, AppConfiguration config, IUrlShortenerProvider shortenerProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config.VendorServices.FirstOrDefault(vnd => vnd.Name == this.Name)
                      ?? throw new ArgumentException(nameof(config));
            _shortener = shortenerProvider?.GetShortenerByName(Constants.Providers.Shorteners.BitLy);
        }

        public string Name => Constants.Providers.Vendors.Amazon;

        public bool CanParse(string content) => AnyMatch(content);

        public bool CanShorten() => _config.ShortenerEnabled;

        public async Task<string> GetDeepLink(string url)
        {
            var builder = new UriBuilder(url);
            if (builder.Query.Length == 1) return url;

            var query = HttpUtility.ParseQueryString(url);
            query["tag"] = _config.AffiliateCustomizer;
            builder.Query = query.ToString();
            var newUrl = builder.ToString();
            if (CanShorten())
                newUrl = (await _shortener.ShortenUrl(newUrl)) ?? newUrl;
            return newUrl;
        }

        public async Task<LinkMessage> ParseContent(string content)
        {
            var title = System.Net.WebUtility.HtmlDecode(TitleRegex.GetMatch(content));
            var pictureUrl = PictureRegex.GetMatch(content);
            var price = PriceRegex.GetMatch(content);
            var saved = PriceSavesRegex.GetMatch(content);
            var originalPrice = OriginalPriceRegex.GetMatch(content);

            return await Task.FromResult(new LinkMessage
            {
                Title = title,
                PictureUrl = pictureUrl,
                FinalPrice = price,
                OriginalPrice = originalPrice,
                SavedPrice = saved
            });
        }

        private bool AnyMatch(string content)
        {
            var result = TitleRegex.IsMatch(content);
            result = result && PictureRegex.IsMatch(content);
            result = result && PriceRegex.IsMatch(content);
            return result;
        }
    }
}