using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Common.Interfaces.Persistence
{
    public interface ITenantRepository : IRepository<Tenant>
    {
        Task<bool> ExistsBySlugAsync(string slug);

        Task<bool> ExistsByNameAsync(string name);

        Task<Tenant?> GetBySlugAsync(string slug);
    }
}
