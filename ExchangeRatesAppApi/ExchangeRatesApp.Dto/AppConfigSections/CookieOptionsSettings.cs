namespace ExchangeRatesApp.Dto.AppConfigSections
{
    public class CookieOptionsSettings
    {
        public required string Key { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public bool Partitioned { get; set; }
        public required string SameSite { get; set; }
    }
}
