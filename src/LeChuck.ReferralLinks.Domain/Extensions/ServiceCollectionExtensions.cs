using AutoMapper;
using LeChuck.DependencyInjection.Extensions;
using LeChuck.ReferralLinks.Application.Services;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Services;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LeChuck.ReferralLinks.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainModule(this IServiceCollection services)
        {
            var assembly = typeof(ServiceCollectionExtensions).Assembly;
            services.AddSingleton<IAuthorizationService, AuthorizationService>();
            services.AddSingleton<IBotAuthorizer, AuthorizationService>();
            services.AddInterface<IHtmlParserStrategy>(assembly);
            services.AddInterface<IUrlShortenerStrategy>(assembly);
            services.AddSingleton<IHtmlParserProvider, HtmlHtmlParserProvider>();
            services.AddSingleton<IUrlShortenerProvider, UrlShortenerProvider>();
            services.AddHttpClient();
            return services;
        }
    }
}
