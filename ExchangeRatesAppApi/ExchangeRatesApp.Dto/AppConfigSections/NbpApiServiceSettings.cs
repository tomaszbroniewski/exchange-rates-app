using System;

namespace ExchangeRatesApp.Dto.AppConfigSections
{
    public class NbpApiServiceSettings : IAppSettingsSection
    {
        public required Uri BaseAddress { get; set; }
    }
}
