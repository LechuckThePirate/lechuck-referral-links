using LeChuck.ReferralLinks.DataAccess.Repositories;
using LeChuck.ReferralLinks.Lambda.Timer.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace LeChuck.ReferralLinks.Lambda.Timer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTimerLambda(this IServiceCollection services)
        {
            services.AddTransient<ISweepProcessor, SweepProcessor>();
            services.AddTransient<ITimedTasksRepository, TimedTasksRepository>();
            return services;
        }
    }
}
