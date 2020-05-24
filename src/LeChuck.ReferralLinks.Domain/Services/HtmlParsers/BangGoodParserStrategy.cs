#region using directives

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Web;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services.HtmlParsers
{
    public class BangGoodParserStrategy : ILinkParserStrategy
    {
        private readonly ILogger<BangGoodParserStrategy> _logger;

        private static readonly Regex ObjectParser = new Regex("<script type=\"application\\/ld\\+json\">(.+?(?=};)})",
            RegexOptions.Singleline);

        private VendorConfig _config;
        private IUrlShortenerStrategy _shortener;

        public BangGoodParserStrategy(ILogger<BangGoodParserStrategy> logger, AppConfiguration config, IUrlShortenerProvider shortenerProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config.VendorServices.FirstOrDefault(vnd => vnd.Name == this.Name)
                      ?? new VendorConfig { Name = this.Name };
            _shortener = shortenerProvider?.GetShortenerByName(Constants.Providers.Shorteners.BitLy) ??
                         throw new ArgumentException(nameof(shortenerProvider));
        }

        public string Name => Constants.Providers.Vendors.BangGood;

        public bool CanParse(string content) => false; // TODO: Fix for single and array prices ObjectParser.IsMatch(content);
        
        public bool CanShorten() => _config.ShortenerEnabled;

        public async Task<string> GetDeepLink(string url)
        {
            var builder = new UriBuilder(_config.AffiliateCustomizer);
            var query = HttpUtility.ParseQueryString(url);
            query["ulp"] = url;
            builder.Query = query.ToString();
            var newUrl = builder.ToString();
            if (CanShorten())
                newUrl = (await _shortener.ShortenUrl(newUrl)) ?? newUrl;
            return newUrl;
        }

        public async Task<LinkMessage> ParseContent(string content)
        {
            var data = GetObjectFromResponse(ObjectParser, content);

            // TODO: Find out best price 
            return await Task.FromResult(new LinkMessage
            {
                Title = data.GetProperty("name").GetString(),
                PictureUrl = data.GetProperty("image").GetString(),
                FinalPrice = $"{data.GetProperty("offers").GetProperty("price").GetString()}" +
                             $"{GetCurrency(data.GetProperty("offers").GetProperty("priceCurrency").GetString())}"
            });
        }

        private JsonElement GetObjectFromResponse(Regex regex, string content)
        {
            var match = regex.Match(content);
            if (match.Groups.Count < 2)
            {
                _logger.LogWarning($"No math '{regex}' in content");
                return default;
            }

            var stringToParse = match.Groups[1].Value.Trim();
            if (string.IsNullOrEmpty(stringToParse))
            {
                _logger.LogWarning("Nothing to parse!");
                return default;
            }

            try
            {
                var pageModule = JsonSerializer.Deserialize<JsonElement>(stringToParse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    DictionaryKeyPolicy = null
                });
                return pageModule;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Could not deserialize the content!\nContent:\n{stringToParse}\n" +
                                   $"Exception: {ex.Message}\n" +
                                   $"StackTrace: {ex.StackTrace}");
                return default;
            }
        }

        private string GetCurrency(string currency)
        {
            return currency switch
            {
                "EUR" => "€",
                "USD" => "$",
                _ => string.Empty
            };
        }
    }
}