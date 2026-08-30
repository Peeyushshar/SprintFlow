using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);

        Task<IReadOnlyList<ApplicationUser>> GetAllAsync(
            CancellationToken cancellationToken = default
        );

        Task DeleteAsync(ApplicationUser? user, CancellationToken cancellationToken);
    }
}
