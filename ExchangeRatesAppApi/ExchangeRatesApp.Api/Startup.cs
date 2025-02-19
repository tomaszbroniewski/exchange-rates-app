using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ExchangeRatesApp.Api.StartupConfigs;
using ExchangeRatesApp.Api.StartupConfigs.Middlewares;
using ExchangeRatesApp.Application;
using ExchangeRatesApp.Infrastructure;

namespace ExchangeRatesApp.Api
{
    /*
     * [DESC]
     * Keeping the old-school Startup schema (removed in .NET 6) still does a good job :)
     */
    public class Startup
    {
        public AppSettings AppSettings { get; }
        public IWebHostEnvironment Environment { get; set; }

        public Startup(IWebHostEnvironment env)
        {
            var cfgBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            AppSettings = new AppSettings(cfgBuilder.Build());
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            StartupApi.Execute(services, AppSettings, Environment);
            StartupApplication.Execute(services);
            StartupDomain.Execute(services);
            StartupInfrastructure.Execute(services, AppSettings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var loggerForHttpRequests = loggerFactory.CreateLogger("HttpRequest");

            app.UseExceptionHandler("/api/error");

            app.UseStaticFiles();

            if (env.IsNotRestricted())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "ExchangeRatesApp API");

                });
            }

            app.UseRouting();

            app.UseCors(CorsConfig.AppCorsPolicy);

            app.UseHttpsRedirection();

            app.UseExchangeRatesAppPollyMiddleware();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseLoggingHttpRequestsDataMiddleware(loggerForHttpRequests);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
