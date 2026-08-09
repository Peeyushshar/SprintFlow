using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;
using SprintFlow.Application.Common.Security;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Features.Authentication.RefreshToken
{
    public class RefreshTokenCommandHandler
        : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IUnitOfWork unitOfWork
        )
        {
            _refreshTokenRepository = refreshTokenRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken
        )
        {
            //-------------------------------------------------
            // Hash supplied refresh token
            //-------------------------------------------------

            var tokenHash = TokenHasher.Hash(request.RefreshToken);

            //-------------------------------------------------
            // Find stored refresh token
            //-------------------------------------------------

            var storedToken = await _refreshTokenRepository.GetByTokenAsync(
                tokenHash,
                cancellationToken
            );

            if (storedToken is null)
            {
                return Result<RefreshTokenResponse>.Failure(AuthErrors.InvalidRefreshToken);
            }

            //-------------------------------------------------
            // Validate refresh token
            //-------------------------------------------------

            if (storedToken.IsRevoked)
            {
                return Result<RefreshTokenResponse>.Failure(AuthErrors.RefreshTokenRevoked);
            }

            if (storedToken.ExpiresAt <= DateTime.UtcNow)
            {
                return Result<RefreshTokenResponse>.Failure(AuthErrors.RefreshTokenExpired);
            }

            //-------------------------------------------------
            // Get user
            //-------------------------------------------------

            var user = storedToken.User;

            if (user is null)
            {
                return Result<RefreshTokenResponse>.Failure(AuthErrors.InvalidRefreshToken);
            }

            if (!user.IsActive)
            {
                return Result<RefreshTokenResponse>.Failure(AuthErrors.InactiveUser);
            }

            //-------------------------------------------------
            // Generate new access token
            //-------------------------------------------------

            var accessToken = await _jwtTokenGenerator.GenerateAccessToken(user);

            //-------------------------------------------------
            // Generate new refresh token
            //-------------------------------------------------

            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            //-------------------------------------------------
            // Revoke old refresh token
            //-------------------------------------------------

            storedToken.RevokedAt = DateTime.UtcNow;

            //-------------------------------------------------
            // Create new refresh token entity
            //-------------------------------------------------

            var newRefreshTokenEntity = new Domain.Entities.RefreshToken
            {
                Id = Guid.NewGuid(),

                UserId = user.Id,

                TokenHash = TokenHasher.Hash(newRefreshToken),

                CreatedAt = DateTime.UtcNow,

                ExpiresAt = _jwtTokenGenerator.GetRefreshTokenExpiry(),
            };

            //-------------------------------------------------
            // Store new refresh token
            //-------------------------------------------------

            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

            //-------------------------------------------------
            // Save changes
            //-------------------------------------------------

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            //-------------------------------------------------
            // Response
            //-------------------------------------------------

            return Result<RefreshTokenResponse>.Success(
                new RefreshTokenResponse
                {
                    UserId = user.Id,

                    TenantId = user.TenantId,

                    AccessToken = accessToken,

                    // Return RAW token to client
                    RefreshToken = newRefreshToken,

                    ExpiresAt = _jwtTokenGenerator.GetAccessTokenExpiry(),
                }
            );
        }
    }
}
