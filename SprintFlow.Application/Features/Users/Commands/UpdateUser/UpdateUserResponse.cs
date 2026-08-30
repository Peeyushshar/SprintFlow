namespace SprintFlow.Application.Features.Users.Commands.UpdateUser
{
    public sealed class UpdateUserResponse
    {
        public Guid UserId { get; init; }
        public Guid? TenantId { get; init; }

        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string? PhoneNumber { get; init; }

        public List<Guid> RoleIds { get; init; } = [];
        public bool IsActive { get; init; }
    }
}
