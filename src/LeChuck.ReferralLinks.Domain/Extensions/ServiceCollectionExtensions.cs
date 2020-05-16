#region using directives

using LeChuck.DependencyInjection.Extensions;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Providers;
using LeChuck.ReferralLinks.Domain.Services;
using LeChuck.ReferralLinks.Domain.Services.ApiClients;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace LeChuck.ReferralLinks.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainModule(this IServiceCollection services)
        {
            var assembly = typeof(ServiceCollectionExtensions).Assembly;

            services.AddScoped<ILinkService, LinkService>();
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<IAdmitadApiClient, AdmitadApiClient>();

            services.AddSingleton<IAuthorizationService, BotAuthorizationService>();
            services.AddSingleton<IBotAuthorizer, BotAuthorizationService>();

            services.AddInterface<ILinkParserStrategy>(assembly);
            services.AddInterface<IAffiliateStrategy>(assembly);
            services.AddInterface<IUrlShortenerStrategy>(assembly);

            services.AddTransient<ILinkParserProvider, LinkParserProvider>();
            services.AddTransient<IAffiliateProvider, AffiliateProvider>();
            services.AddTransient<IUrlShortenerProvider, UrlShortenerProvider>();

            services.AddHttpClient();
            return services;
        }
    }
}