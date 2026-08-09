using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Common.Interfaces.Persistence
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

        Task<RefreshToken?> GetByTokenAsync(
            string token,
            CancellationToken cancellationToken = default
        );

        Task RevokeAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    }
}
