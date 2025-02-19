using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace ExchangeRatesApp.Api.StartupConfigs.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingHttpRequestsDataMiddleware(this IApplicationBuilder builder, ILogger loggerForHttpRequests)
        {
            return builder.UseMiddleware<LoggingHttpRequestsDataMiddleware>(loggerForHttpRequests);
        }

        public static IApplicationBuilder UseExchangeRatesAppPollyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PollyMiddleware>();
        }
    }
}
