using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SprintFlow.Application.Common.Security;
using SprintFlow.Application.Features.Projects.Commands.Create;
using SprintFlow.Application.Features.Projects.Commands.Delete;
using SprintFlow.Application.Features.Projects.Queries.GetProject;
using SprintFlow.Application.Features.Projects.Update;

namespace SprintFlow.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [Authorize(Policy = AuthorizationPolicies.TenantUser)]
    public class ProjectsController : ControllerBase
    {
        private readonly ISender _sender;

        public ProjectsController(ISender sender)
        {
            _sender = sender;
        }

        //Create API
        [HttpPost]
        public async Task<IActionResult> CreateProjectAsync(
            CreateProjectCommand command,
            CancellationToken cancellationToken
        )
        {
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        //Update API
        [HttpPut]
        public async Task<IActionResult> UpdateProjectAsync(
            UpdateProjectCommand command,
            CancellationToken cancellationToken
        )
        {
            var result = await _sender.Send(command, cancellationToken);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectsAsync(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetProjectsQuery(), cancellationToken);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetProjectByIdQuery(id), cancellationToken);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProjectAsync(
            Guid id,
            CancellationToken cancellationToken
        )
        {
            var result = await _sender.Send(new DeleteProjectCommand(id), cancellationToken);

            return result.IsSuccess ? NoContent() : NotFound(result);
        }
    }
}
