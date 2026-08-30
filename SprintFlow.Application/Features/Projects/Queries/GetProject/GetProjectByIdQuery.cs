using MediatR;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Projects.Queries.GetProject
{
    public record GetProjectByIdQuery(Guid ProjectId) : IRequest<Result<GetProjectsResponse>>;
}
