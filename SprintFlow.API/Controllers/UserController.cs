using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SprintFlow.Application.Features.Users.Commands.ChangeUserStatus;
using SprintFlow.Application.Features.Users.Commands.CreateUser;
using SprintFlow.Application.Features.Users.Commands.UpdateUser;
using SprintFlow.Application.Features.Users.Queries.GetUserById;
using SprintFlow.Application.Features.Users.Queries.GetUsers;

namespace SprintFlow.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public sealed class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        // -------------------------------------------------
        // Create
        // -------------------------------------------------

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            var result = await _sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Created($"/api/users/{result.Value!.Id}", result);
        }

        // -------------------------------------------------
        // Update
        // -------------------------------------------------

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            // Prevent route/body mismatch
            if (id != command.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            var result = await _sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // -------------------------------------------------
        // Get all
        // -------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUsersQuery(), cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // -------------------------------------------------
        // Get by ID
        // -------------------------------------------------

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUserByIdQuery(id), cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        // -------------------------------------------------
        // Change status
        // -------------------------------------------------

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus(
            Guid id,
            ChangeUserStatusCommand command,
            CancellationToken cancellationToken
        )
        {
            if (id != command.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            var result = await _sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
