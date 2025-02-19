using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace ExchangeRatesApp.Infrastructure.Database
{
    public class DesignTimeExchangeRatesAppDbContextFactory : IDesignTimeDbContextFactory<ExchangeRatesAppDbContext>
    {
        public ExchangeRatesAppDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine("DesignTimeDbAppContextFactory is running...");
            Console.WriteLine("Conn string is being used from appsettings.design.json");

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.design.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ExchangeRatesAppDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("MAIN"));

            return new ExchangeRatesAppDbContext(optionsBuilder.Options);
        }
    }
}
