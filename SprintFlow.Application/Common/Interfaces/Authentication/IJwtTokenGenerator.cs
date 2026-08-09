using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateAccessToken(ApplicationUser user);

        string GenerateRefreshToken();

        DateTime GetAccessTokenExpiry();

        DateTime GetRefreshTokenExpiry();
    }
}
