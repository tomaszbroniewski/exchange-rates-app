using ExchangeRatesApp.Dto.AppConfigSections;

namespace ExchangeRatesApp.Application.TechnicalInterfaces
{
    public interface IAppSettings
    {
        string MainConnectionString { get; }
        MainSettings MainSettings { get; }
        PollySettings PollySettings { get; }
        AuthSettings AuthSettings { get; }
        NbpApiServiceSettings NbpApiServiceSettings { get; }
    }
}
