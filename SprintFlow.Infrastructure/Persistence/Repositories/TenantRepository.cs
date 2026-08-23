using Microsoft.EntityFrameworkCore;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence.Repositories
{
    public sealed class TenantRepository : Repository<Tenant>, ITenantRepository
    {
        public TenantRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<bool> ExistsBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default
        )
        {
            return await DbSet.AnyAsync(tenant => tenant.Slug == slug, cancellationToken);
        }

        public async Task<IReadOnlyList<Tenant>> GetAllTenantsAsync(
            CancellationToken cancellationToken = default
        )
        {
            return await GetAll()
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
