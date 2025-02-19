using ExchangeRatesApp.Application.TechnicalInterfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Application.UserAccount
{
    public class LogoutFromUserAccountQueryHandler : IRequestHandler<LogoutFromUserAccountQuery, Unit>
    {
        private readonly IAuthCookieService _authCookieService;

        public LogoutFromUserAccountQueryHandler(IAuthCookieService authCookieService)
        {
            _authCookieService = authCookieService;
        }

        public Task<Unit> Handle(LogoutFromUserAccountQuery query, CancellationToken cancellationToken)
        {
            _authCookieService.RemoveAuthTokenCookie();
            return Task.FromResult(Unit.Value);
        }
    }

    public class LogoutFromUserAccountQuery : IRequest<Unit>
    {
    }
}
