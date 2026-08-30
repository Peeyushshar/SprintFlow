using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Common.Interfaces.Persistence
{
    public interface ITenantRepository : IRepository<Tenant>
    {
        Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Tenant>> GetAllTenantsAsync(
            CancellationToken cancellationToken = default
        );
    }
}
