using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SprintFlow.Application.Features.TenantManagement.Command.Create;
using SprintFlow.Application.Features.TenantManagement.Command.Delete;
using SprintFlow.Application.Features.TenantManagement.Command.Update;
using SprintFlow.Application.Features.TenantManagement.Queries.GetAll;
using SprintFlow.Application.Features.TenantManagement.Queries.GetById;

namespace SprintFlow.API.Controllers
{
    [ApiController]
    [Route("api/tenants")]
    [Authorize(Roles = "Admin")]
    public sealed class TenantsController : ControllerBase
    {
        private readonly ISender _sender;

        public TenantsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateTenantCommand command,
            CancellationToken cancellationToken
        )
        {
            var result = await _sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Created($"/api/tenants/{result.Value!.TenantId}", result);
        }

        [HttpPut("{tenantId:guid}")]
        public async Task<IActionResult> UpdateAsync(
            Guid tenantId,
            [FromBody] UpdateTenantCommand command,
            CancellationToken cancellationToken
        )
        {
            // Make route ID the source of truth
            command = command with
            {
                TenantId = tenantId,
            };

            var result = await _sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTenantAsync(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetTenantQuery(), cancellationToken);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTenantByIdAsync(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var result = await _sender.Send(new GetTenantByIdQuery(id), cancellationToken);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTenantAsync(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var result = await _sender.Send(new DeleteTenantCommand(id), cancellationToken);

            return result.IsSuccess ? NoContent() : NotFound(result);
        }
    }
}
