namespace SprintFlow.Application.Features.Authentication.Register
{
    public class RegisterResponse
    {
        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
