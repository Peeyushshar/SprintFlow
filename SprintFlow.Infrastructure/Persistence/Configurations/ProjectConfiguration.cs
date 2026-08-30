using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintFlow.Domain.Constants;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable(DatabaseConstants.TablePrefix + "Projects");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(150);

            builder.Property(p => p.Key).IsRequired().HasMaxLength(20);

            builder.Property(p => p.Description).HasMaxLength(1000);

            builder.Property(p => p.IsActive).IsRequired();

            // Tenant → Projects
            builder
                .HasOne(p => p.Tenant)
                .WithMany(t => t.Projects)
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Project → Creator
            builder
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Project Key should be unique within a tenant
            builder.HasIndex(p => new { p.TenantId, p.Key }).IsUnique();

            // Tenant filtering
            builder.HasIndex(p => p.TenantId);
        }
    }
}
