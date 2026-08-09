namespace SprintFlow.Application.Common.Interfaces.Persistence;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}