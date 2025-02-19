using EntityFrameworkCore.Testing.Moq;
using ExchangeRatesApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesApp.Tests.Common
{
    public static class DatabaseHelpers
    {
        public static ExchangeRatesAppDbContext CreateInMemoryDbContext(this BaseTestContainer baseCtr, object currentTestClass)
        {
            var options = new DbContextOptionsBuilder<ExchangeRatesAppDbContext>()
                .UseInMemoryDatabase(databaseName: currentTestClass.GetType().Name)
                .Options;

            var dbContext = Create.MockedDbContextFor<ExchangeRatesAppDbContext>(options);

            // we can add seed data here

            dbContext.SaveChanges();

            return dbContext;
        }

        public static void ClearDatabase(this ExchangeRatesAppDbContext dbContext)
        {
            dbContext.RemoveRange(dbContext.Users);

            dbContext.SaveChanges();
        }
    }
}
