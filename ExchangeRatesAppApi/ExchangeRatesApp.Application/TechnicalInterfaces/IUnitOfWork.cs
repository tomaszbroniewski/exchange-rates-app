using System.Threading.Tasks;
using System.Transactions;
using ExchangeRatesApp.Domain;

namespace ExchangeRatesApp.Application.TechnicalInterfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        Task CommitCurrentTransactionAsync();
        IBaseRepository<T> CreateRepository<T>() where T : class;
        TransactionScope CreateTransactionScope(int? minutesTimeout = null, TransactionScopeOption option = TransactionScopeOption.Required);
    }
}