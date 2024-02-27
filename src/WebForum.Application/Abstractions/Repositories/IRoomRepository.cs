using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Repositories;

public interface IRoomRepository : IRepository<Room, Guid>
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
}