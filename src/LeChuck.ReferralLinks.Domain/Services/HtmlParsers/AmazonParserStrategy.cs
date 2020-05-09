using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Extensions;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Models;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Domain.Services.HtmlParsers
{
    public class AmazonParserStrategy : IHtmlParserStrategy
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AmazonParserStrategy> _logger;

        private static Regex AmazonHostRegex = new Regex(@"http[s]?:.*(amazon\.es\/|amzn\.to\/|amazon\.com\/)");

        private static Regex TitleRegex = new Regex("<h1 id=\"title\".+?(?=productTitle).+?(?=>)>(.+?(?=<\\/span>))", RegexOptions.Singleline);
        // https://regex101.com/r/z3VH13/2
        private static Regex PictureRegex = new Regex("<div id=\"imgTagWrapperId\".+?(?=https\\:\\/\\/)(.+?(?=\"))", RegexOptions.Singleline);
        private static Regex PriceRegex = new Regex("<span id=\"priceblock_(ourprice|dealprice)\".+?(?=>)>(.+?(?=<))");
        private static Regex OriginalPriceRegex = new Regex("<span class=\"priceBlockStrikePriceString.+?(?=>)>(.+?(?=<))", RegexOptions.Singleline);
        private static Regex PriceSavesRegex = new Regex("priceBlockSavingsString\">(.+?(?=<))", RegexOptions.Singleline);

        public AmazonParserStrategy(ILogger<AmazonParserStrategy> logger, IHttpClientFactory httpClientFactory)
        {
            if (httpClientFactory == null) throw new ArgumentNullException(nameof(httpClientFactory));
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool CanParse(string url)
        {
            var host = new Uri(url).Host?.ToLower();
            if (string.IsNullOrEmpty(host))
                return false;
            var result = AmazonHostRegex.IsMatch(url);
            return result;
        }

        public async Task<LinkData> ParseUrl(string url)
        {
            var content = await _httpClient.GetStringAsync(url);

            var title = TitleRegex.GetMatch(content);
            var pictureUrl = PictureRegex.GetMatch(content);
            var price = PriceRegex.GetMatch(content);
            var saved = PriceSavesRegex.GetMatch(content);
            var originalPrice = OriginalPriceRegex.GetMatch(content);

            return new LinkData
            {
                Title = title,
                PictureUrl = pictureUrl,
                LongUrl = url,
                FinalPrice = price,
                OriginalPrice = originalPrice,
                SavedPrice = saved
            };
        }

    }
}

/*
 
   
Ahorras
<td class="a-span12 a-color-price a-size-base priceBlockSavingsString">
            
                
                
                    6,45&nbsp;€ (32%)
                
            
            
        </td>





    */
