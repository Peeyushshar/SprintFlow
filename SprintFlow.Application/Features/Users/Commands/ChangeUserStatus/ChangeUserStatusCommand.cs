using SprintFlow.Application.Common.Interfaces.CQRS;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Users.Commands.ChangeUserStatus
{
    public record ChangeUserStatusCommand(Guid UserId, bool IsActive)
        : ICommand<Result<ChangeUserStatusResponse>>;
}
