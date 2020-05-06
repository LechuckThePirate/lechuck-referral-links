using AutoMapper;
using LeChuck.DependencyInjection.Extensions;
using LeChuck.ReferralLinks.Application.Services;
using LeChuck.ReferralLinks.Domain.Interfaces;
using LeChuck.ReferralLinks.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LeChuck.ReferralLinks.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            return services;
        }
    }
}
