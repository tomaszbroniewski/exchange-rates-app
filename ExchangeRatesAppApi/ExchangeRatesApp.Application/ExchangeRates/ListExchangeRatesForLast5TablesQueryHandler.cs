using ExchangeRatesApp.Domain.ExchangeRates;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Application.ExchangeRates
{
    public class ListExchangeRatesForLast5TablesQueryHandler : IRequestHandler<ListExchangeRatesForLast5TablesQuery, ICollection<ExchangeRatesTable>>
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;

        public ListExchangeRatesForLast5TablesQueryHandler(IExchangeRateRepository exchangeRateRepository)
        {
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task<ICollection<ExchangeRatesTable>> Handle(ListExchangeRatesForLast5TablesQuery query, CancellationToken cancellationToken)
        {
            return await _exchangeRateRepository.FetchExchangeRatesForLast5ATablesAsync();
        }
    }

    public class ListExchangeRatesForLast5TablesQuery : IRequest<ICollection<ExchangeRatesTable>>
    {
    }
}
