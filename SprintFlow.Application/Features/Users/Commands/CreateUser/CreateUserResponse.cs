namespace SprintFlow.Application.Features.Users.Commands.CreateUser
{
    public sealed class CreateUserResponse
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; init; }

        public string FirstName { get; init; } = string.Empty;

        public string LastName { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public string? PhoneNumber { get; init; } = string.Empty;

        public List<Guid> RoleIds { get; init; } = new List<Guid>();
    }
}
