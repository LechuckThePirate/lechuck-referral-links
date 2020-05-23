#region using directives

using System.Globalization;
using AutoMapper;
using LeChuck.ReferralLinks.Crosscutting.Extensions;
using LeChuck.ReferralLinks.DataAccess.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

namespace LeChuck.ReferralLinks.Webhook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson();
            services.AddAutoMapper(configAction: cfg => { },
                typeof(MultiLinkDbEntity).Assembly);
            services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Debug).AddConsole());
            services.AddApplication(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var cultureInfo = new CultureInfo("es-ES");
            cultureInfo.NumberFormat.CurrencySymbol = "€";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}