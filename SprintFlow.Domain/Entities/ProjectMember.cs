using SprintFlow.Domain.Common;
using SprintFlow.Domain.Constants;

namespace SprintFlow.Domain.Entities
{
    public class ProjectMember : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }

        public Guid ProjectId { get; set; }

        public Guid UserId { get; set; }

        public ProjectMemberRole Role { get; set; }

        // Navigation properties
        public Project Project { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}
