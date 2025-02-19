namespace ExchangeRatesApp.Application.TechnicalInterfaces
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        string? UserId { get; }
    }
}
