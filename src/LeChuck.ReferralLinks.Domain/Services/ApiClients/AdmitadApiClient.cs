using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.UnitsOfWork;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Domain.Services.ApiClients
{
    public class AdmitadApiClient : IAdmitadApiClient
    {
        private readonly AppConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfigUnitOfWork _configUnitOfWork;
        private readonly ILogger<AdmitadApiClient> _logger;
        private readonly AffiliateServiceConfig _admitad;

        private class AuthResponse
        {
            public string access_token { get; set; }
            public int? expires_in { get; set; }
            public string token_type { get; set; }
            public string refresh_token { get; set; }
        }

        public AdmitadApiClient(AppConfiguration config, IHttpClientFactory clientFactory, 
            IConfigUnitOfWork configUnitOfWork, ILogger<AdmitadApiClient> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _configUnitOfWork = configUnitOfWork ?? throw new ArgumentNullException(nameof(configUnitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _admitad = _config.GetAffiliateConfig(Constants.Providers.Affiliates.Admitad);

        }

        public async Task<bool> Authenticate()
        {
            if (HasValidCredentials())
                return true;

            var credentials = _admitad.Credentials;

            using var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = GetAuthHeaderValue(credentials);
            using var request = GetAuthRequest(_admitad.AuthEndpoint, credentials);

            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(await response.Content.ReadAsStringAsync());
                await UpdateConfig(authResponse, credentials);
                return true;
            }
            _logger.LogError($"Failed to authenticate to Admitad: {response.StatusCode.ToString()}\n" +
                             $"{JsonSerializer.Serialize(response)}");
            return false;
        }

        public async Task<string[]> DeepLink(int spaceId, int campaignId, string url)
        {
            if (!HasValidCredentials() && !(await Authenticate()))
            {
                throw new AuthenticationException();
            }
                
            var credentials = _admitad.Credentials;

            string endpoint =
                $"{_admitad.ApiEndpoint}deeplink/{spaceId}/advcampaign/{campaignId}/?ulp={UrlEncoder.Default.Encode(url)}";

            using var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(credentials.TokenType, credentials.AccessToken);
            var response = await httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HttpResponse error: {response.StatusCode.ToString()}");
            }

            var payload = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<string[]>(payload);
            return data;
        }

        private AuthenticationHeaderValue GetAuthHeaderValue(ApiCredentials credentials)
        {
            var headerValue = $"{credentials.ClientId}:{credentials.ClientSecret}";
            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes(headerValue));
            return new AuthenticationHeaderValue(credentials.AuthType,base64String);
        }

        private HttpRequestMessage GetAuthRequest(string url, ApiCredentials credentials)
        {
            return new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("client_id", credentials.ClientId),
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("scope",
                        "deeplink_generator websites advcampaigns_for_website")
                })
            };
        }

        private async Task UpdateConfig(AuthResponse authResponse, ApiCredentials credentials)
        {
            credentials.AccessToken = authResponse.access_token;
            credentials.TokenType = authResponse.token_type;
            if (authResponse.expires_in.HasValue)
            {
                credentials.Expires = DateTime.Now
                    .AddSeconds(authResponse.expires_in ?? 0)
                    .AddHours(-1); // let's give an hour margin for the token to expire
            }
            credentials.RefreshToken = authResponse.refresh_token;

            await _configUnitOfWork.SaveConfig(_config);
        }

        private bool HasValidCredentials()
        {
            var credentials = _admitad.Credentials;

            if (!string.IsNullOrWhiteSpace(credentials.AccessToken)
                && DateTime.Now < credentials.Expires)
                return true;

            return false;
        }
    }
}
