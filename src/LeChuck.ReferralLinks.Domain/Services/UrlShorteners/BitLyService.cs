#region using directives

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Enums;
using LeChuck.ReferralLinks.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

#endregion

namespace LeChuck.ReferralLinks.Domain.Services.UrlShorteners
{
    public class BitLyService : IUrlShortenerStrategy
    {
        private readonly HttpClient _client;

        private readonly string _token;
        private readonly string _endpoint;

        public BitLyService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            if (clientFactory == null) throw new ArgumentNullException(nameof(clientFactory));
            _client = clientFactory.CreateClient();
            _token = Environment.GetEnvironmentVariable(Constants.BitLyTokenValueName) ??
                     throw new ArgumentException(nameof(_token));
            _endpoint = Environment.GetEnvironmentVariable(Constants.BitLyEndpointValueName) ??
                        throw new ArgumentException(nameof(_endpoint));
        }

        public UrlShortenersEnum Key => UrlShortenersEnum.BitLy;

        public async Task<string> ShortenUrl(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            var content = new StringContent(JsonConvert.SerializeObject(
                new
                {
                    long_url = url
                }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_endpoint, content);
            dynamic result = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            return result.link;
        }
    }
}