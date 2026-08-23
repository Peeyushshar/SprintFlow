using Microsoft.EntityFrameworkCore;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetAllAsync(
            CancellationToken cancellationToken
        )
        {
            return await _context
                .ApplicationUsers.AsNoTracking()
                .OrderBy(x => x.Email)
                .ToListAsync(cancellationToken);
        }

        public async Task<ApplicationUser?> GetByIdAsync(
            Guid userId,
            CancellationToken cancellationToken
        )
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken
            );
        }

        public async Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.ApplicationUsers.AnyAsync(x => x.Id == userId, cancellationToken);
        }

        public Task<bool> ExistsByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
