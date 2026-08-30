namespace SprintFlow.Application.Features.Users.Commands.ChangeUserStatus
{
    public sealed class ChangeUserStatusResponse
    {
        public Guid UserId { get; init; }
        public bool IsActive { get; init; }
    }
}
