using ExchangeRatesApp.Api.Helpers;
using ExchangeRatesApp.Application.ExchangeRates;
using ExchangeRatesApp.Application.TechnicalInterfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Api.Controllers
{
    public class ExchangeRatesController : ApiControllerBase
    {
        public ExchangeRatesController(ICurrentUser currentUser, IMediator mediator) : base(currentUser, mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> ListExchangeRatesForLast5Tables()
        {
            var exchangeRates = await _mediator.Send(new ListExchangeRatesForLast5TablesQuery());
            return Ok(exchangeRates);
        }
    }
}
