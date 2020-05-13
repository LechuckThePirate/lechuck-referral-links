using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LeChuck.ReferralLinks.Domain.Services.HtmlParsers
{
    public class AliExpressParserStrategy : ILinkParserStrategy
    {
        private readonly ILogger<AliExpressParserStrategy> _logger;
        private readonly HttpClient _httpClient;

        static readonly Regex PageModuleRegex = new Regex("\"pageModule\":(.+?(?=,\"preSaleModule\"))"); 
        static readonly Regex PriceModuleRegex = new Regex("\"priceModule\":(.+?(?=,\"quantityModule\"))");  
        public AliExpressParserStrategy(ILogger<AliExpressParserStrategy> logger, IHttpClientFactory httpClientFactory)
        {
            if (httpClientFactory == null) throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClientFactory.CreateClient();
        }

        public bool CanParse(string url) 
        {
            try
            {
                var host = new Uri(url).Host?.ToLower();
                if (string.IsNullOrEmpty(host))
                    return false;

                return host.EndsWith(".aliexpress.com") || host.EndsWith("ali.ski") || host.EndsWith("alitems.com");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        public async Task<Link> ParseUrl(string url)
        {
            var content = await GetContent(url);

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

            return await Task.FromResult(new Link
            {
                Title = title,
                FinalPrice = price,
                PictureUrl = pictureUrl,
                LongUrl = url
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

        private async Task<string> GetContent(string url)
        {
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

    }
}
