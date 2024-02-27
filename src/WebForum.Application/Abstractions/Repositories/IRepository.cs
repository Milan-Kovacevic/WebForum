namespace WebForum.Application.Abstractions.Repositories;

public interface IRepository<TEntity, in TEntityId> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}