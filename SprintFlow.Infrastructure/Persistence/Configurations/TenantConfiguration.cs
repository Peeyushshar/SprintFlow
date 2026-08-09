using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintFlow.Domain.Constants;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable(DatabaseConstants.TablePrefix + "Tenants");
            builder.HasIndex(t => t.Slug).IsUnique();
        }
    }
}
