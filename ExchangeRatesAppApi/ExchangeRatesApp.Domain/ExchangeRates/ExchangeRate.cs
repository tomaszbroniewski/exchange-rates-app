namespace ExchangeRatesApp.Domain.ExchangeRates
{
    public class ExchangeRate
    {
        public required string Currency { get; set; }
        public required string Code { get; set; }
        public decimal Mid { get; set; }
    }
}
