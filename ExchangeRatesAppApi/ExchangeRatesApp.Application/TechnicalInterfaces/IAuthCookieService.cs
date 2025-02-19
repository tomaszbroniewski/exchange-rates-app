using Microsoft.IdentityModel.Tokens;

namespace ExchangeRatesApp.Application.TechnicalInterfaces
{
    public interface IAuthCookieService
    {
        void RemoveAuthTokenCookie();
        void SetAuthTokenCookie(SecurityToken token);
    }
}
