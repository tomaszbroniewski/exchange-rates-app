using System;

namespace ExchangeRatesApp.Dto.AppConfigSections
{
    public class MainSettings : IAppSettingsSection
    {
        public TimeSpan DbTransactionTimeout { get; set; }
        public string ClientAddress { get; set; } = string.Empty;
    }
}
