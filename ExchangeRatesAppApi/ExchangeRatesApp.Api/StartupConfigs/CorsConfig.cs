using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRatesApp.Api.StartupConfigs
{
    public static class CorsConfig
    {
        public readonly static string AppCorsPolicy = "AppCorsPolicy";
        public readonly static string[] AllowedHeaders = new string[] { "Authorization", "Content-Type", "Access-Control-Allow-Origin" };
        public readonly static string[] AllowedMethods = new string[] { "GET", "POST", "PUT", "DELETE", "HEAD", "PATCH" };

        public static void AddAppCors(this IServiceCollection services, IWebHostEnvironment environment, AppSettings appSettings)
        {
            if (environment.IsNotRestricted())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(AppCorsPolicy, builder => builder
                                .WithOrigins(appSettings.MainSettings.ClientAddress)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .SetIsOriginAllowed(t => true)
                                .AllowCredentials());

                    options.DefaultPolicyName = AppCorsPolicy;
                });
            }
            else
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(AppCorsPolicy, builder => builder
                                .WithOrigins(appSettings.MainSettings.ClientAddress)
                                .WithMethods(AllowedMethods)
                                .WithHeaders(AllowedHeaders)
                                .AllowCredentials());

                    options.DefaultPolicyName = AppCorsPolicy;
                });

            }
        }
    }
}
