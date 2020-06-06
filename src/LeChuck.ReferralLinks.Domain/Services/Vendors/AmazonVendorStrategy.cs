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

namespace LeChuck.ReferralLinks.Domain.Services.Vendors
{
    public class AmazonVendorStrategy : IVendorStrategy
    {
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

        private readonly VendorConfig _config;
        private readonly IUrlShortenerStrategy _shortener;

        public AmazonVendorStrategy(AppConfiguration config, IUrlShortenerProvider shortenerProvider)
        {
            _config = config.VendorServices.FirstOrDefault(vnd => vnd.Name == this.Name)
                      ?? throw new ArgumentException(nameof(config));
            _shortener = shortenerProvider?.GetShortenerByName(Constants.Providers.Shorteners.BitLy);
        }

        public string Name => Constants.Providers.Vendors.Amazon;

        public bool CanParse(string content) => AnyMatch(content);

        public bool CanShorten() => _config.ShortenerEnabled;

        public async Task<string> GetDeepLink(string url)
        {
            if (url == null) return null;

            if (url.IndexOf("/ref=", StringComparison.InvariantCultureIgnoreCase) != 0)
                url = url.Substring(0, url.IndexOf("/ref=", StringComparison.InvariantCultureIgnoreCase));

            var builder = new UriBuilder(url);
            if (!url.IsShortUrl())
            {
                var query = HttpUtility.ParseQueryString(url);
                query["tag"] = _config.AffiliateCustomizer;
                builder.Query = query.ToString();
                var newUrl = builder.Uri.ToString();
                if (CanShorten())
                    newUrl = (await _shortener.ShortenUrl(newUrl)) ?? newUrl;
                return newUrl;
            }

            return url;
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