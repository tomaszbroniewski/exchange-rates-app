using ExchangeRatesApp.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRatesApp.Api.StartupConfigs
{
    public static class DbMigrationsConfig
    {
        public static void ExecuteDbMigrations(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ExchangeRatesAppDbContext>();

            // For the sample ExchangeRates app to be more convenient, it automatically creates the database; normally, database is created by the environment.
            context.Database.EnsureCreated();

            // For the sample ExchangeRates app to be more convenient, it automatically seeds sample data; normally, EF migrations are used here.
            ExchangeRatesAppDbContext.Initialize(context);
        }
    }
}
