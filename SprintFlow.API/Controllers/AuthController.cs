using MediatR;
using Microsoft.AspNetCore.Mvc;
using SprintFlow.Application.Features.Authentication.Login;
using SprintFlow.Application.Features.Authentication.Logout;
using SprintFlow.Application.Features.Authentication.RefreshToken;

namespace SprintFlow.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _mediator;

        public AuthController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            LogoutCommand command,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return Unauthorized(result.Error);
            }

            return NoContent();
        }
    }
}
