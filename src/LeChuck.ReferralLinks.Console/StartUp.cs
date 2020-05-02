using System;
using System.Threading.Tasks;
using LeChuck.ReferralLinks.Application;
using LeChuck.ReferralLinks.Application.Extensions;
using LeChuck.ReferralLinks.DataAccess.Extensions;
using LeChuck.ReferralLinks.Domain.Extensions;
using LeChuck.Rifas.Application;
using LeChuck.Telegram.Bot.FrameWork.Extensions;
using LeChuck.Telegram.Bot.Framework.Models;
using LeChuck.Telegram.Bot.Framework.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;

namespace LeChuck.ReferralLinks.Console
{
    public class StartUp
    {
        private readonly ProcessTimer _timer;
        private readonly IConfigurationBuilder _configurationBuilder = new ConfigurationBuilder();

        private IWebhookProcessor _processor;
        private ITelegramBotClient _botClient;
        private IServiceProvider _serviceProvider;
        private ILogger<StartUp> _logger;

        public StartUp(ProcessTimer stopwatch)
        {
            _timer = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        }

        private IConfiguration Configuration { get; set; }
        private AppConfiguration AppConfiguration { get; } = new AppConfiguration();
        private IServiceCollection Services { get; } = new ServiceCollection();

        public StartUp ConfigureApplication()
        {
            _timer.Mark("Configuration", () =>
            {
                Configuration = _configurationBuilder
                    .AddJsonFile("appsettings.json", false)
                    .Build();
                Configuration.Bind("AppConfiguration", AppConfiguration);

                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                };
            });

            return this;
        }

        public StartUp ConfigureServices()
        {
            _timer.Mark("Register services", () =>
            {
                Services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Debug).AddProvider(new OneLineLoggerProvider()).AddDebug());

                Services
                    .AddTelegramBotFramework(new TelegramBotFrameworkConfiguration
                    {
                        BotKey = Environment.GetEnvironmentVariable("BOT_KEY"),
                        CommandPrefix = AppConfiguration.CommandPrefix
                    }, Commands.CommandModels)
                    .AddApplicationModule()
                    .AddDomainModule()
                    .AddDataAccessModule(Configuration);

                _serviceProvider = Services.BuildServiceProvider();
                _logger = _serviceProvider.GetService<ILogger<StartUp>>();
                Services.AddSingleton(_serviceProvider);
            });

            return this;
        }

        public StartUp LoadServices()
        {
            _timer.Mark("Load Services", () =>
            {
                _processor = _serviceProvider.GetRequiredService<IWebhookProcessor>();
                _botClient = _serviceProvider.GetRequiredService<ITelegramBotClient>();
                _botClient.OnUpdate += OnUpdate;
            });
            return this;
        }

        public async Task Run()
        {
            _timer.Mark("Init Bot", () =>
            {
                _botClient.StartReceiving();
            });
            await Task.CompletedTask;
        }

        private void OnUpdate(object sender, UpdateEventArgs e)
        {
            try
            {
                _processor.HandleUpdateAsync(e.Update).Wait();
            }
            catch (Exception ex) when (ex.InnerException != null && ex.InnerException is ChatNotInitiatedException)
            {
                _botClient.SendTextMessageAsync(chatId: e.Update.Message?.Chat.Id ?? e.Update.CallbackQuery.From.Id,
                    "No puedo iniciar una conversacion contigo si no me activas primero: Haz click aqui @vaperafflebot y vuelve a probar.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Unhandled exception!", ex);
            }
        }

    }
}
