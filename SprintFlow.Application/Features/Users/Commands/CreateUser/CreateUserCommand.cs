using SprintFlow.Application.Common.Interfaces.CQRS;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string PhoneNumber,
        List<Guid> RoleIds
    ) : ICommand<Result<CreateUserResponse>>;
}
