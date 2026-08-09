using MediatR;
using Microsoft.Extensions.Logging;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;
using SprintFlow.Application.Common.Security;

namespace SprintFlow.Application.Features.Authentication.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LogoutCommandHandler> _logger;

        public LogoutCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            ILogger<LogoutCommandHandler> logger
        )
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            //-------------------------------------------------
            // Hash refresh token
            //-------------------------------------------------

            var tokenHash = TokenHasher.Hash(request.RefreshToken);

            //-------------------------------------------------
            // Find refresh token
            //-------------------------------------------------

            var storedToken = await _refreshTokenRepository.GetByTokenAsync(
                tokenHash,
                cancellationToken
            );

            if (storedToken is null)
            {
                return Result.Failure(AuthErrors.InvalidRefreshToken);
            }

            //-------------------------------------------------
            // Check if already revoked
            //-------------------------------------------------

            if (storedToken.IsRevoked)
            {
                return Result.Failure(AuthErrors.RefreshTokenRevoked);
            }

            //-------------------------------------------------
            // Revoke refresh token
            //-------------------------------------------------

            storedToken.RevokedAt = DateTime.UtcNow;

            //-------------------------------------------------
            // Save changes
            //-------------------------------------------------

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {UserId} logged out successfully.", storedToken.UserId);

            return Result.Success();
        }
    }
}
