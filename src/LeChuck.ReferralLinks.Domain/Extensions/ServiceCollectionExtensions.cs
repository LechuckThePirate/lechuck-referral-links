#region using directives

using LeChuck.DependencyInjection.Extensions;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Providers;
using LeChuck.ReferralLinks.Domain.Services;
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

            services.AddScoped<IChannelService, ChannelService>();

            services.AddSingleton<IAuthorizationService, BotAuthorizationService>();
            services.AddSingleton<IBotAuthorizer, BotAuthorizationService>();

            services.AddInterface<IVendorStrategy>(assembly);
            services.AddInterface<IUrlShortenerStrategy>(assembly);

            services.AddTransient<IVendorProvider, VendorProvider>();
            services.AddTransient<IUrlShortenerProvider, UrlShortenerProvider>();

            services.AddHttpClient();
            return services;
        }
    }
}