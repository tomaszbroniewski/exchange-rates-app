using ExchangeRatesApp.Api.Helpers;
using ExchangeRatesApp.Application.TechnicalInterfaces;
using ExchangeRatesApp.Application.UserAccount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Api.Controllers
{
    public class UserAccountController : ApiControllerBase
    {
        public UserAccountController(ICurrentUser currentUser, IMediator mediator) : base(currentUser, mediator)
        {
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<LoginToUserAccountResponse> LoginToUserAccount(LoginToUserAccountCommand command)
        {
            return await _mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _mediator.Send(new LogoutFromUserAccountQuery());
            return Ok();
        }
    }
}
