using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Common.Interfaces.Persistence
{
    public interface IProjectRepository
    {
        Task<bool> ExistsByKeyAsync(string key, CancellationToken cancellationToken);

        Task<bool> ExistsByKeyExceptAsync(
            string key,
            Guid projectId,
            CancellationToken cancellationToken
        );

        Task AddAsync(Project project, CancellationToken cancellationToken);

        Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken);
    }
}
