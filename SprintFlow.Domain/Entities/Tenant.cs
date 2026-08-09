using SprintFlow.Domain.Common;

namespace SprintFlow.Domain.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string SubscriptionPlan { get; set; } = "Free";

    public bool IsActive { get; set; } = true;

    // Navigation Property
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}