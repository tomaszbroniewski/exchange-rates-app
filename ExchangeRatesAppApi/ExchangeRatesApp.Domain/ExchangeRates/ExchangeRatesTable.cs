using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExchangeRatesApp.Domain.ExchangeRates
{
    public class ExchangeRatesTable
    {
        public required List<ExchangeRate> Rates { get; set; }
    }
}
