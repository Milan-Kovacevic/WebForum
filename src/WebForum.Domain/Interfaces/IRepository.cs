namespace WebForum.Domain.Interfaces;

public interface IRepository<TEntity, in TEntityId> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken);
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    IQueryable<TEntity> GetQueryable();
}