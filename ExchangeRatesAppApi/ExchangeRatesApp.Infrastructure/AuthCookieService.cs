using ExchangeRatesApp.Application.TechnicalInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace ExchangeRatesApp.Infrastructure
{
    public class AuthCookieService : IAuthCookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppSettings _appSettings;

        public AuthCookieService(IHttpContextAccessor httpContextAccessor, IAppSettings appSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings;
        }

        public void SetAuthTokenCookie(SecurityToken token)
        {
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var cookieHeader = BuildCookieHeader(tokenString);
            _httpContextAccessor.HttpContext?.Response.Headers.Append("Set-Cookie", cookieHeader);
        }

        public void RemoveAuthTokenCookie()
        {
            var cookieHeader = BuildCookieHeader("") + "; Expires=Thu, 01 Jan 1970 00:00:00 GMT";
            _httpContextAccessor.HttpContext?.Response.Headers.Append("Set-Cookie", cookieHeader);
        }

        private string BuildCookieHeader(string token)
        {
            var options = _appSettings.AuthSettings.CookieOptionsSettings;

            return $"{options.Key}={token}; Path=/; Secure={options.Secure}; HttpOnly={options.HttpOnly}; SameSite={options.SameSite}" +
                   (options.Partitioned ? "; Partitioned" : "");
        }
    }
}
