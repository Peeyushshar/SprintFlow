using SprintFlow.Application.Common.MultiTenancy;

namespace SprintFlow.Infrastructure.Persistence
{
    public sealed class DesignTimeCurrentTenant : ICurrentTenant
    {
        public Guid? Id => null;

        public bool IsAvailable => false;
    }
}
