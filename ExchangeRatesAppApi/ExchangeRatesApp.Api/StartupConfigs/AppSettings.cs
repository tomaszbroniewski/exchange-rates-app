using ExchangeRatesApp.Application.TechnicalInterfaces;
using ExchangeRatesApp.Dto.AppConfigSections;
using Microsoft.Extensions.Configuration;

namespace ExchangeRatesApp.Api.StartupConfigs
{
    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration _configuration;
        public IConfiguration Configuration => _configuration;

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string MainConnectionString => _configuration.GetConnectionString("MAIN")!;

        public MainSettings MainSettings => _configuration.GetTypedConfigSection<MainSettings>();
        public PollySettings PollySettings => _configuration.GetTypedConfigSection<PollySettings>();
        public AuthSettings AuthSettings => _configuration.GetTypedConfigSection<AuthSettings>();
        public NbpApiServiceSettings NbpApiServiceSettings => _configuration.GetTypedConfigSection<NbpApiServiceSettings>();
    }
}
