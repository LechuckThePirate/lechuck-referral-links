using System;
using LeChuck.ReferralLinks.Application;
using LeChuck.ReferralLinks.Application.Extensions;
using LeChuck.ReferralLinks.Application.UpdateHandlers;
using LeChuck.ReferralLinks.DataAccess.Extensions;
using LeChuck.ReferralLinks.Domain;
using LeChuck.ReferralLinks.Domain.Contracts.UnitsOfWork;
using LeChuck.ReferralLinks.Domain.Extensions;
using LeChuck.ReferralLinks.Domain.Models;
using LeChuck.ReferralLinks.Domain.Services.HtmlParsers;
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
                .AddTelegramBotFramework(new TelegramBotFrameworkConfiguration
                {
                    BotKey = botKey,
                    // TODO Move to repo
                    CommandPrefix = "/"
                }, Commands.CommandModels, typeof(LinkUpdateHandler).Assembly)
                .AddDefaultAWSOptions(configuration.GetAWSOptions())
                .AddApplicationModule()
                .AddDomainModule()
                .AddDataAccessModule(configuration);

            services.AddSingleton(configuration);
            
            var appConfig = GetConfiguration(services);
            services.AddSingleton(appConfig);
            services.AddSingleton(services.BuildServiceProvider());
            ServiceProvider = services.BuildServiceProvider();
        }

        private static AppConfiguration GetConfiguration(IServiceCollection services)
        {
            var repo = services.BuildServiceProvider().GetService<IConfigUnitOfWork>();
            var config = repo.LoadConfig().GetAwaiter().GetResult();
            return config ?? new AppConfiguration();
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

    }
}
