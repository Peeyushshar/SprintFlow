using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintFlow.Domain.Constants;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence.Configurations
{
    public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.ToTable(DatabaseConstants.TablePrefix + "ProjectMembers");

            builder.HasKey(pm => pm.Id);

            builder.Property(pm => pm.Role).IsRequired();

            // Tenant → ProjectMembers
            builder
                .HasOne(pm => pm.Project)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → ProjectMembers
            builder
                .HasOne(pm => pm.User)
                .WithMany()
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicate membership
            builder.HasIndex(pm => new { pm.ProjectId, pm.UserId }).IsUnique();

            // Tenant filtering
            builder.HasIndex(pm => pm.TenantId);

            // Optional additional protection
            builder.HasIndex(pm => new
            {
                pm.TenantId,
                pm.ProjectId,
                pm.UserId,
            });
        }
    }
}
