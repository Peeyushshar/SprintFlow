using Microsoft.AspNetCore.Identity;

namespace SprintFlow.Application.Features.Users.Queries
{
    public class GetUsersResponse
    {
        public Guid Id { get; init; }
        public Guid? TenantId { get; init; }
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public bool IsDeleted { get; init; }
        public List<string> Roles { get; init; }
    }
}
