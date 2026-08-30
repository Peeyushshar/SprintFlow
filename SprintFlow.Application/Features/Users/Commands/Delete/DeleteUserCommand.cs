using SprintFlow.Application.Common.Interfaces.CQRS;

namespace SprintFlow.Application.Features.Users.Commands.Delete
{
    public record DeleteUserCommand(Guid UserId) : ICommand;
}
