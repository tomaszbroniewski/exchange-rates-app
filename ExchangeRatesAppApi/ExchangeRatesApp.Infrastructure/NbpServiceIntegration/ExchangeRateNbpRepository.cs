using ExchangeRatesApp.Application.TechnicalInterfaces;
using ExchangeRatesApp.Domain.ExchangeRates;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Infrastructure.NbpServiceIntegration
{
    public class ExchangeRateNbpRepository : IExchangeRateRepository
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateNbpRepository(HttpClient httpClient, IAppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.NbpApiServiceSettings.BaseAddress, "exchangerates/");
        }

        public async Task<List<ExchangeRatesTable>> FetchExchangeRatesForLast5ATablesAsync()
        {
            var response = await _httpClient.GetStringAsync("tables/a/last/5/");
            var exchangeTables = JsonSerializer.Deserialize<List<ExchangeRatesTable>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return exchangeTables ?? new List<ExchangeRatesTable>();
        }
    }
}
