namespace ExchangeRatesApp.Dto.AppConfigSections
{
    public class AuthSettings : IAppSettingsSection
    {
        public string Key { get; set; } = null!;
        public int ExpireMinutes { get; set; }
        public int ClockSkewMinutes { get; set; }
        public required CookieOptionsSettings CookieOptionsSettings { get; set; }
    }
}
