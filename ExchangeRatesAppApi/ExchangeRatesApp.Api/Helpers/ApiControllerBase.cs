using ExchangeRatesApp.Application.TechnicalInterfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRatesApp.Api.Helpers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly ICurrentUser _currentUser;
        protected readonly IMediator _mediator;

        protected ApiControllerBase(ICurrentUser currentUser, IMediator mediator)
        {
            _currentUser = currentUser;
            _mediator = mediator;
        }
    }
}
