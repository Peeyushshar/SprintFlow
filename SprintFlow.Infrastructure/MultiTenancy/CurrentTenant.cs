using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.MultiTenancy;

namespace SprintFlow.Infrastructure.MultiTenancy
{
    public sealed class CurrentTenant : ICurrentTenant
    {
        private readonly ICurrentUserService _currentUserService;

        public CurrentTenant(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public Guid? Id => _currentUserService.TenantId;

        public bool IsAvailable => Id.HasValue;
    }
}
