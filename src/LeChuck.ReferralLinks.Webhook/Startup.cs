using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Mappers;
using LeChuck.ReferralLinks.Application;
using LeChuck.ReferralLinks.Application.Extensions;
using LeChuck.ReferralLinks.DataAccess.Extensions;
using LeChuck.ReferralLinks.Domain.Extensions;
using LeChuck.Rifas.Application;
using LeChuck.Telegram.Bot.FrameWork.Extensions;
using LeChuck.Telegram.Bot.Framework.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LeChuck.ReferralLinks.Webhook
{
    public class Startup
    {
        public const string AppS3BucketKey = "AppS3Bucket";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Debug).AddConsole());

            var appConfiguration = new AppConfiguration();
            Configuration.Bind("AppConfiguration", appConfiguration);

            services
                .AddTelegramBotFramework(new TelegramBotFrameworkConfiguration
                {
                    BotKey = Environment.GetEnvironmentVariable("BOT_KEY"),
                    CommandPrefix = appConfiguration.CommandPrefix
                }, Commands.CommandModels)
                .AddApplicationModule()
                .AddDomainModule()
                .AddDataAccessModule(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
