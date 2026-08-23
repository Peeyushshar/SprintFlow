namespace SprintFlow.Application.Features.TenantManagement.Command.Create
{
    public sealed class CreateTenantResponse
    {
        public Guid TenantId { get; init; }

        public string TenantName { get; init; } = string.Empty;

        public string Slug { get; init; } = string.Empty;

        public string SubscriptionPlan { get; init; } = string.Empty;

        public Guid OwnerId { get; init; }

        public string OwnerEmail { get; init; } = string.Empty;
    }
}
