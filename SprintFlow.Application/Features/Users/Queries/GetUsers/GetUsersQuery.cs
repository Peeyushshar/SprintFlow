using MediatR;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Users.Queries.GetUsers
{
    public record GetUsersQuery : IRequest<Result<IReadOnlyList<GetUsersResponse>>>;
}
