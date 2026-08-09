namespace SprintFlow.Application.Features.Authentication.RefreshToken
{
    public class RefreshTokenResponse
    {
        public Guid UserId { get; set; }

        public Guid? TenantId { get; set; }

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }
    }
}
