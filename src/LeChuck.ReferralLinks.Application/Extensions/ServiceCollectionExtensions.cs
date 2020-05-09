using LeChuck.DependencyInjection.Extensions;
using LeChuck.ReferralLinks.Application.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LeChuck.ReferralLinks.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<ILinkView, LinkView>();
            return services;
        }
    }
}
