using System;
using LeChuck.ReferralLinks.Application;
using LeChuck.ReferralLinks.Application.Extensions;
using LeChuck.ReferralLinks.Application.UpdateHandlers;
using LeChuck.ReferralLinks.DataAccess.Extensions;
using LeChuck.ReferralLinks.Domain;
using LeChuck.ReferralLinks.Domain.Extensions;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.UnitsOfWork;
using LeChuck.Telegram.Bot.FrameWork.Extensions;
using LeChuck.Telegram.Bot.Framework.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace LeChuck.ReferralLinks.Crosscutting.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var botKey = configuration.GetSection(Constants.TelegramTokenValueName).Value;

            services
                .AddDefaultAWSOptions(configuration.GetAWSOptions())
                .AddApplicationModule()
                .AddDomainModule()
                .AddDataAccessModule(configuration);

            services.AddSingleton(configuration);
            
            var appConfig = GetConfiguration(services, () => GetDefaultConfig(configuration));
            services.AddSingleton(appConfig);

            services
                .AddTelegramBotFramework(new TelegramBotFrameworkConfiguration
                {
                    BotKey = botKey,
                    CommandPrefix = appConfig.CommandPrefix
                }, Commands.CommandModels, typeof(LinkUpdateHandler).Assembly);

            services.AddSingleton(services.BuildServiceProvider());
            ServiceProvider = services.BuildServiceProvider();
        }

        private static AppConfiguration GetConfiguration(IServiceCollection services, Func<AppConfiguration> defaultConfig)
        {
            var repo = services.BuildServiceProvider().GetService<IConfigUnitOfWork>();
            var config = repo.LoadConfig().GetAwaiter().GetResult();
            if (config == null)
            {
                config = defaultConfig.Invoke();
                repo.SaveConfig(config).GetAwaiter();
            }
            return config;
        }

        public static void ConfigureApplication()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };
        }

        static AppConfiguration GetDefaultConfig(IConfiguration configuration)
        {
            var rootUserId = configuration.GetSection(Constants.TelegramRootUserId).Value;
            var result = new AppConfiguration {RootUserId = rootUserId};
            return result;
        }

    }
}
