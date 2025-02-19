using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ExchangeRatesApp.Infrastructure
{
    public static class StartupDomain
    {
        public static void Execute(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // add required startup items for domain layer
        }
    }
}
