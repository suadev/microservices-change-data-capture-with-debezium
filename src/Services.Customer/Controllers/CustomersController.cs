using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Customer.Commands;

namespace Services.Customer.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{email}")]
        public async Task<ActionResult> UpdateCustomer(string email, [FromBody] UpdateCustomerCommand command)
            => Ok(await _mediator.Send(command.SetEmail(email)));
    }
}