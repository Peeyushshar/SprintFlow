using MediatR;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Projects.Queries.GetProject
{
    public record GetProjectsQuery : IRequest<Result<IReadOnlyList<GetProjectsResponse>>>;
}
