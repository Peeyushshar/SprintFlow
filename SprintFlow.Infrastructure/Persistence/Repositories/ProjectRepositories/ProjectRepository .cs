using Microsoft.EntityFrameworkCore;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence.Repositories.ProjectRepositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByKeyAsync(string key, CancellationToken cancellationToken)
        {
            return await _context.Projects.AnyAsync(
                project => project.Key == key,
                cancellationToken
            );
        }

        public async Task<bool> ExistsByKeyExceptAsync(
            string key,
            Guid projectId,
            CancellationToken cancellationToken
        )
        {
            return await _context.Projects.AnyAsync(
                project => project.Key == key && project.Id != projectId,
                cancellationToken
            );
        }

        public async Task AddAsync(Project project, CancellationToken cancellationToken)
        {
            await _context.Projects.AddAsync(project, cancellationToken);
        }

        public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Projects.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context
                .Projects.AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
