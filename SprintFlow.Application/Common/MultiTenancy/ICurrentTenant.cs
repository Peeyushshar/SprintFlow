namespace SprintFlow.Application.Common.MultiTenancy
{
    public interface ICurrentTenant
    {
        Guid? Id { get; }

        bool IsAvailable { get; }
    }
}
