using MediatR;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<Result<GetUsersResponse>>;
}
