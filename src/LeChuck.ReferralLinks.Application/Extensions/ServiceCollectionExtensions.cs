using LeChuck.DependencyInjection.Extensions;
using LeChuck.ReferralLinks.Application.StateMachines;
using LeChuck.ReferralLinks.Application.StateMachines.ProgramLink;
using LeChuck.ReferralLinks.Application.StateMachines.Strategies;
using LeChuck.ReferralLinks.Application.Views;
using LeChuck.Stateless.StateMachine;
using LeChuck.Stateless.StateMachine.Extensions;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LeChuck.ReferralLinks.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<ILinkView, LinkView>();
            services.AddTransient<IStateMachine<IUpdateContext>, ProgramLinkStateMachine>();
            services.AddTransient<IStateMachineStore, StateMachineStore>();
            services.AddStateMachines();
            services.AddInterface<ILinkDataStrategy>();
            services.AddTransient<ILinkDataStrategySelector, LinkDataStrategySelector>();
            return services;
        }
    }
}
