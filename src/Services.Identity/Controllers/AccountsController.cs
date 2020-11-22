using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Identity.Commands;

namespace Services.Identity.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserCommand command)
            => Ok(await _mediator.Send(command));
    }
}