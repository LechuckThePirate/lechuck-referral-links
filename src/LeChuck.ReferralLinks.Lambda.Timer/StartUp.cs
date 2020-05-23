#region using directives

using System;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using LeChuck.ReferralLinks.Domain;
using LeChuck.ReferralLinks.Lambda.Timer.Extensions;
using LeChuck.ReferralLinks.Lambda.Timer.Processors;
using LeChuck.Telegram.Bot.FrameWork.Extensions;
using LeChuck.Telegram.Bot.Framework.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace LeChuck.ReferralLinks.Lambda.Timer
{
    public class StartUp
    {
        private readonly ProcessTimer _timer;
        private readonly IConfigurationBuilder _configurationBuilder = new ConfigurationBuilder();
        private ISweepProcessor _sweepProcessor;

        public StartUp(ProcessTimer stopwatch = null)
        {
            _timer = stopwatch ?? new ProcessTimer(true);
        }

        private IConfiguration Configuration { get; set; }
        private IServiceCollection Services { get; } = new ServiceCollection();

        public StartUp ConfigureApplication()
        {
            _timer.Mark("Configuration", () =>
            {
                Configuration = _configurationBuilder
                    .AddJsonFile("appsettings.json", false)
                    .AddSystemsManager("/ReferralLink",
                        new AWSOptions {Region = RegionEndpoint.EUWest1})
                    .Build();
            });

            return this;
        }

        public StartUp ConfigureServices()
        {
            _timer.Mark("Register services", () =>
            {
                Services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Debug).AddDebug());
                Services.AddTelegramBotFramework(new TelegramBotFrameworkConfiguration
                {
                    BotKey = Environment.GetEnvironmentVariable(Constants.TelegramTokenValueName)
                }, commandModels: new CommandModel[] { });
                Services.AddTimerLambda();
            });

            return this;
        }

        public StartUp LoadServices()
        {
            _timer.Mark("Load Services",
                () => { _sweepProcessor = Services.BuildServiceProvider().GetRequiredService<ISweepProcessor>(); });
            return this;
        }

        public void Run()
        {
            _timer.Mark("Run Sweep", async () => { await _sweepProcessor.Sweep(); });
        }
    }
}