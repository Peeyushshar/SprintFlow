using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintFlow.Domain.Constants;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable(DatabaseConstants.TablePrefix + "RefreshTokens");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TokenHash).IsRequired().HasMaxLength(128);

            builder.HasIndex(x => x.TokenHash).IsUnique();

            builder.Property(x => x.ExpiresAt).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
