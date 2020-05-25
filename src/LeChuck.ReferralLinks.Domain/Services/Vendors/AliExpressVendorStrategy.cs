#region using directives

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services.Vendors
{
    public class AliExpressVendorStrategy : IVendorStrategy
    {
        private readonly ILogger<AliExpressVendorStrategy> _logger;
        private readonly IUrlShortenerStrategy _shortener;
        private readonly VendorConfig _config;

        static readonly Regex PageModuleRegex = new Regex("\"pageModule\":(.+?(?=,\"preSaleModule\"))");
        static readonly Regex PriceModuleRegex = new Regex("\"priceModule\":(.+?(?=,\"quantityModule\"))");

        public AliExpressVendorStrategy(ILogger<AliExpressVendorStrategy> logger, AppConfiguration config, IUrlShortenerProvider shortenerProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config.VendorServices.FirstOrDefault(vnd => vnd.Name == this.Name)
                      ?? new VendorConfig {Name = this.Name};
            _shortener = shortenerProvider?.GetShortenerByName(Constants.Providers.Shorteners.BitLy) ?? 
                throw new ArgumentException(nameof(shortenerProvider));
        }

        public string Name => Constants.Providers.Vendors.AliExpress;

        public bool CanParse(string content) => PageModuleRegex.IsMatch(content) && PriceModuleRegex.IsMatch(content);

        public bool CanShorten() => _config.ShortenerEnabled;

        public async Task<string> GetDeepLink(string url)
        {
            var path = new UriBuilder(url).Path;
            if (!Regex.IsMatch(path, @"^\/[a-z,A-Z,0-9,-]{5,7}$"))
            {
                var builder = new UriBuilder(_config.AffiliateCustomizer);
                var query = HttpUtility.ParseQueryString(url);
                query["ulp"] = url;
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
            var pageModule = GetObjectFromResponse("pageModule", PageModuleRegex, content);
            var priceModule = GetObjectFromResponse("priceModule", PriceModuleRegex, content);

            if (pageModule == null || priceModule == null)
            {
                _logger.LogDebug(content);
                return null;
            }

            var pictureUrl = pageModule.imagePath;
            var title = pageModule.title;
            var price = priceModule.formatedActivityPrice;
            var originalPrice = priceModule.formatedPrice;
            var discount = priceModule.discount;

            return await Task.FromResult(new LinkMessage
            {
                Title = title,
                FinalPrice = price,
                PictureUrl = pictureUrl,
                OriginalPrice = originalPrice,
                SavedPrice = discount
            });
        }

        private dynamic GetObjectFromResponse(string objname, Regex regex, string content)
        {
            var match = regex.Match(content);
            if (match.Groups.Count < 2)
            {
                _logger.LogWarning($"No math '{regex}' in content for {objname}");
                return null;
            }

            var stringToParse = match.Groups[1].Value;
            if (string.IsNullOrEmpty(stringToParse))
            {
                _logger.LogWarning($"Nothing to parse for {objname}!");
                return null;
            }

            try
            {
                dynamic pageModule = JsonConvert.DeserializeObject(stringToParse);
                return pageModule;
            }
            catch (Exception)
            {
                _logger.LogWarning($"Could not deserialize the content of {objname}!\nContent:\n{stringToParse}");
                return null;
            }
        }
    }
}