using Microsoft.EntityFrameworkCore;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            RefreshToken refreshToken,
            CancellationToken cancellationToken = default
        )
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }

        public async Task<RefreshToken?> GetByTokenAsync(
            string token,
            CancellationToken cancellationToken = default
        )
        {
            return await _context
                .RefreshTokens.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.TokenHash == token, cancellationToken);
        }

        public Task RevokeAsync(
            RefreshToken refreshToken,
            CancellationToken cancellationToken = default
        )
        {
            refreshToken.RevokedAt = DateTime.UtcNow;

            _context.RefreshTokens.Update(refreshToken);

            return Task.CompletedTask;
        }
    }
}
