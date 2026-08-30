using SprintFlow.Domain.Common;

namespace SprintFlow.Domain.Entities
{
    public class Project : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties

        public Tenant Tenant { get; set; } = null!;

        public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    }
}
