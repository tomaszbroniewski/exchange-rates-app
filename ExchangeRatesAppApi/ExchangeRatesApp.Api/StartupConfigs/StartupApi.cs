using ExchangeRatesApp.Api.Helpers;
using ExchangeRatesApp.Api.StartupConfigs.Middlewares;
using ExchangeRatesApp.Application.TechnicalInterfaces;
using ExchangeRatesApp.Domain.UserAccount;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using System;

namespace ExchangeRatesApp.Api.StartupConfigs
{
    public static class StartupApi
    {
        public static void Execute(IServiceCollection services, AppSettings appSettings, IWebHostEnvironment environment)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddAppCors(environment, appSettings);

            services.AddAppAuthentication(appSettings);

            services.AddMvc();

            services.AddHttpContextAccessor();

            AddPolly(services, appSettings);
            services.AddSingleton<IAppSettings, AppSettings>();
            services.AddScoped<ICurrentUser, HttpCurrentUser>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddExchangeRatesAppSwagger();
        }

        private static void AddPolly(IServiceCollection services, AppSettings appSettings)
        {
            services.AddResiliencePipeline(PollyMiddleware.PollyPipelineKey, builder =>
            {
                builder
                    .AddRetry(PollyMiddleware.PollyRetryStrategyOptionsFactory(appSettings))
                    .AddCircuitBreaker(new CircuitBreakerStrategyOptions());
            });
        }
    }
}
