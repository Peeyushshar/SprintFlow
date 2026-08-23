using Microsoft.EntityFrameworkCore;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Domain.Common;

namespace SprintFlow.Infrastructure.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(ApplicationDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default
        )
        {
            return await DbSet.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
        }

        public virtual async Task CreateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default
        )
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public virtual Task UpdateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default
        )
        {
            DbSet.Update(entity);

            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(
            TEntity entity,
            CancellationToken cancellationToken = default
        )
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            Context.Update(entity);

            return Task.CompletedTask;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet.AsQueryable();
        }
    }
}
