using System;
using System.Threading.Tasks;
using System.Transactions;
using ExchangeRatesApp.Application.TechnicalInterfaces;
using ExchangeRatesApp.Domain;

namespace ExchangeRatesApp.Infrastructure.Database.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IAppSettings _appConfig;
        private readonly ExchangeRatesAppDbContext _dbContext;

        public UnitOfWork(IAppSettings appSettings, ExchangeRatesAppDbContext dbContext)
        {
            _appConfig = appSettings;
            _dbContext = dbContext;
        }

        public IBaseRepository<T> CreateRepository<T>()
            where T : class
        {
            return new Repository<T>(_dbContext);
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task CommitCurrentTransactionAsync()
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await _dbContext.Database.CurrentTransaction.CommitAsync();
            }
        }

        /// <summary>
        /// Creates async safe transaction scope with specified timeout
        /// </summary>
        /// <returns></returns>
        public TransactionScope CreateTransactionScope(int? minutesTimeout = null, TransactionScopeOption option = TransactionScopeOption.Required)
        {
            return new TransactionScope(option,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = minutesTimeout.HasValue && minutesTimeout.Value > 0 ? TimeSpan.FromMinutes(minutesTimeout.Value) : _appConfig.MainSettings.DbTransactionTimeout },
                TransactionScopeAsyncFlowOption.Enabled
                );
        }
    }
}
