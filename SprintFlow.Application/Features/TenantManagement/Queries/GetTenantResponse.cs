namespace SprintFlow.Application.Features.TenantManagement.Queries
{
    public class GetTenantResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Slug { get; init; } = null!;
        public string? Description { get; init; }
        public string? SubscriptionPlan { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public bool IsDeleted { get; init; }
    }
}
