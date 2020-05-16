#region using directives

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public AmazonParserStrategy(ILogger<AmazonParserStrategy> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string ParserName => Constants.Providers.Vendors.Amazon;

        public bool CanParse(string content) => AnyMatch(content);

        public async Task<Link> ParseContent(string content)
        {
            var title = TitleRegex.GetMatch(content);
            var pictureUrl = PictureRegex.GetMatch(content);
            var price = PriceRegex.GetMatch(content);
            var saved = PriceSavesRegex.GetMatch(content);
            var originalPrice = OriginalPriceRegex.GetMatch(content);

            return await Task.FromResult(new Link
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