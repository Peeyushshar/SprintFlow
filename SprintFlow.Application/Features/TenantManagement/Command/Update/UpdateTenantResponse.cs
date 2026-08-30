namespace SprintFlow.Application.Features.TenantManagement.Command.Update
{
    public sealed class UpdateTenantResponse
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Slug { get; init; } = string.Empty;

        public string SubscriptionPlan { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
}
