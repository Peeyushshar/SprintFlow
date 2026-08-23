using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<IReadOnlyList<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken);
        Task<ApplicationUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken);
    }
}
