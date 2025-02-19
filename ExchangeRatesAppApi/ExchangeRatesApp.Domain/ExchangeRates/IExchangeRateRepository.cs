using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Domain.ExchangeRates
{
    public interface IExchangeRateRepository
    {
        Task<List<ExchangeRatesTable>> FetchExchangeRatesForLast5ATablesAsync();
    }
}
