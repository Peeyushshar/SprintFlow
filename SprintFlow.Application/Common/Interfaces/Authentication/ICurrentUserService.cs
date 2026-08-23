namespace SprintFlow.Application.Common.Interfaces.Authentication
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }

        Guid? TenantId { get; }

        bool IsAuthenticated { get; }
    }
}
