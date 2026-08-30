using Microsoft.EntityFrameworkCore;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UserRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        // -------------------------------------------------
        // Get all users
        // -------------------------------------------------

        public async Task<IReadOnlyList<ApplicationUser>> GetAllAsync(
            CancellationToken cancellationToken = default
        )
        {
            var tenantId = _currentUserService.TenantId;

            if (tenantId is null)
            {
                return [];
            }

            return await _context
                .ApplicationUsers.AsNoTracking()
                .Where(x => x.TenantId == tenantId)
                .OrderBy(x => x.Email)
                .ToListAsync(cancellationToken);
        }

        // -------------------------------------------------
        // Get user by ID
        // -------------------------------------------------

        public async Task<ApplicationUser?> GetByIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken
            );
        }

        // -------------------------------------------------
        // Delete user
        // -------------------------------------------------

        public Task DeleteAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            user.DeletedBy = _currentUserService.UserId;

            return Task.CompletedTask;
        }
    }
}
