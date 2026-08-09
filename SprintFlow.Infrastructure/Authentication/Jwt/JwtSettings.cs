namespace SprintFlow.Infrastructure.Authentication.Jwt;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    // Access Token expiry (minutes)
    public int AccessTokenExpiryMinutes { get; set; }

    // Refresh Token expiry (days)
    public int RefreshTokenExpiryDays { get; set; }
}