#region using directives

using LeChuck.DependencyInjection.Extensions;
using LeChuck.ReferralLinks.Application.Services;
using LeChuck.ReferralLinks.Application.StateMachines;
using LeChuck.ReferralLinks.Application.StateMachines.Config;
using LeChuck.ReferralLinks.Application.StateMachines.Config.ConfigMachine;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData;
using LeChuck.ReferralLinks.Application.StateMachines.LinkData.ProgramLinkMachine;
using LeChuck.ReferralLinks.Application.Views;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.Stateless.StateMachine;
using LeChuck.Stateless.StateMachine.Extensions;
using LeChuck.Telegram.Bot.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace LeChuck.ReferralLinks.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<ILinkView, LinkView>();
            services.AddTransient<IStateMachine<IUpdateContext, MultiLinkMessage>, ProgramLinkStateMachine>();
            services.AddTransient<IStateMachine<IUpdateContext, AppConfiguration>, ConfigStateMachine>();
            services.AddTransient<IMultiLinkMessageBuilder, MultiLinkMessageBuilder>();
            services.AddTransient<IStateMachineStore, StateMachineStore>();
            services.AddStateMachines();
            services.AddInterface<IMultiLinkStrategy>();
            services.AddTransient<IStateMachineStrategySelector<IUpdateContext, MultiLinkMessage>, LinkDataStrategySelector>();
            services.AddInterface<IConfigStrategy>();
            services
                .AddTransient<IStateMachineStrategySelector<IUpdateContext, AppConfiguration>, ConfigStrategySelector
                >();
            return services;
        }
    }
}