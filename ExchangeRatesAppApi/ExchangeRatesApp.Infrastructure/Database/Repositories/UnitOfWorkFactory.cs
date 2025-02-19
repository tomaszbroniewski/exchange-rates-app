using ExchangeRatesApp.Application.TechnicalInterfaces;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRatesApp.Infrastructure.Database.Repositories
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IAppSettings _appConfig;
        private readonly DbContextOptions<ExchangeRatesAppDbContext> _dbExchangeRatesAppOptions;

        public UnitOfWorkFactory(IAppSettings appSettings, DbContextOptions<ExchangeRatesAppDbContext> dbExchangeRatesAppOptions)
        {
            _appConfig = appSettings;
            _dbExchangeRatesAppOptions = dbExchangeRatesAppOptions;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_appConfig, new ExchangeRatesAppDbContext(_dbExchangeRatesAppOptions));
        }
    }
}
