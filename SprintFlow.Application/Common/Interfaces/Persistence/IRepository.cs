using SprintFlow.Domain.Common;

namespace SprintFlow.Application.Common.Interfaces.Persistence
{
    public interface IRepository<TEntity>
    where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(Guid id);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        IQueryable<TEntity> Query();
    }
}
