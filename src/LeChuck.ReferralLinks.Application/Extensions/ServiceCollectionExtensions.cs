using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace LeChuck.ReferralLinks.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddAutoMapper(assemblies: typeof(ServiceCollectionExtensions).Assembly);
            return services;
        }
    }
}
