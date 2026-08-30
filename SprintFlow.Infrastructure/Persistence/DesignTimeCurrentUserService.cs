using SprintFlow.Application.Common.Interfaces.Authentication;

namespace SprintFlow.Infrastructure.Persistence
{
    public sealed class DesignTimeCurrentUserService : ICurrentUserService
    {
        public Guid? UserId => null;

        public Guid? TenantId => null;

        public bool IsAuthenticated => false;
    }
}
