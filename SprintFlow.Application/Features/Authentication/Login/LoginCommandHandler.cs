using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;
using SprintFlow.Application.Common.Security;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Features.Authentication.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<LoginCommandHandler> _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            ILogger<LoginCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;   
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            //-------------------------------------------------
            // Find user
            //-------------------------------------------------

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                _logger.LogWarning(
                    "Login failed. User not found for email {Email}",
                    request.Email);

                return Result<LoginResponse>.Failure(
                    AuthErrors.InvalidCredentials);
            }

            //-------------------------------------------------
            // Check account status
            //-------------------------------------------------

            if (!user.IsActive)
            {
                return Result<LoginResponse>.Failure(
                    AuthErrors.InactiveUser);
            }

            //-------------------------------------------------
            // Check password
            //-------------------------------------------------

            var passwordValid = await _userManager.CheckPasswordAsync(
                user,
                request.Password);

            if (!passwordValid)
            {
                _logger.LogWarning(
                    "Login failed. Invalid password for user {UserId}",
                    user.Id);

                return Result<LoginResponse>.Failure(
                    AuthErrors.InvalidCredentials);
            }

            //-------------------------------------------------
            // Generate Access Token
            //-------------------------------------------------

            var accessToken =
                await _jwtTokenGenerator.GenerateAccessToken(user);

            //-------------------------------------------------
            // Generate Refresh Token
            //-------------------------------------------------

            var refreshToken =
                _jwtTokenGenerator.GenerateRefreshToken();

            //-------------------------------------------------
            // Get Access Token expiry
            //-------------------------------------------------

            var expiresAt =
                _jwtTokenGenerator.GetAccessTokenExpiry();

            //-------------------------------------------------
            // Create Refresh Token Entity
            //-------------------------------------------------

            var refreshTokenEntity = new Domain.Entities.RefreshToken
            {
                Id = Guid.NewGuid(),

                UserId = user.Id,

                // Store HASH, not the raw refresh token
                TokenHash = TokenHasher.Hash(refreshToken),

                CreatedAt = DateTime.UtcNow,

                ExpiresAt =
                    _jwtTokenGenerator.GetRefreshTokenExpiry()
            };

            //-------------------------------------------------
            // Save Refresh Token
            //-------------------------------------------------

            await _refreshTokenRepository.AddAsync(
                refreshTokenEntity,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            //-------------------------------------------------
            // Response
            //-------------------------------------------------

            return Result<LoginResponse>.Success(
                new LoginResponse
                {
                    UserId = user.Id,
                    TenantId = user.TenantId!.Value,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt
                });
        }
    }
}
