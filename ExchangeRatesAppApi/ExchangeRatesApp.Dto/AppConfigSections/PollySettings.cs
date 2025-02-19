using System;

namespace ExchangeRatesApp.Dto.AppConfigSections
{
    public class PollySettings : IAppSettingsSection
    {
        public int MaxRetryAttempts { get; set; } = 1;
        public TimeSpan DelayBetweenRetries { get; set; } = TimeSpan.FromSeconds(2);
    }
}
