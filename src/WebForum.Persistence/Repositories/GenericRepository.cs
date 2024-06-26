using Microsoft.EntityFrameworkCore;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public abstract class GenericRepository<TEntity, TEntityId>(ApplicationDbContext context)
    : IRepository<TEntity, TEntityId> where TEntity : class
{
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await context.Set<TEntity>().FindAsync([id], cancellationToken: cancellationToken);
    }

    public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
    }
}