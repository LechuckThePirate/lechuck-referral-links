using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using LeChuck.ReferralLinks.Crosscutting.Extensions;
using LeChuck.Telegram.Bot.Framework.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        public StartUp(ProcessTimer stopwatch)
        {
            _timer = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        }

        private IConfiguration Configuration { get; set; }
        private IServiceCollection Services { get; } = new ServiceCollection();

        public StartUp ConfigureApplication()
        {
            _timer.Mark("Configuration", () =>
            {
                Configuration = _configurationBuilder
                    .AddJsonFile("appsettings.json", false)
                    .AddSystemsManager($"/ReferralLink",
                        new AWSOptions { Region = RegionEndpoint.EUWest1 }, 
                        reloadAfter: TimeSpan.FromMinutes(1))
                    .Build();
            });

            ApplicationExtensions.ConfigureApplication();

            return this;
        }

        public StartUp ConfigureServices()
        {
            _timer.Mark("Register services", () =>
            {
                Services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Debug).AddProvider(new OneLineLoggerProvider()).AddDebug());
                Services.AddApplication(Configuration);
            });

            return this;
        }

        public StartUp LoadServices()
        {
            _timer.Mark("Load Services", () =>
            {
                _processor = ApplicationExtensions.ServiceProvider.GetRequiredService<IWebhookProcessor>();
                _botClient = ApplicationExtensions.ServiceProvider.GetRequiredService<ITelegramBotClient>();
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
