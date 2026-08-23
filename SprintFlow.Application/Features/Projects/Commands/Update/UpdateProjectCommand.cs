using MediatR;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Projects.Update
{
    public record UpdateProjectCommand(Guid ProjectId, string Name, string Key, string? Description)
        : IRequest<Result<UpdateProjectResponse>>;
}
