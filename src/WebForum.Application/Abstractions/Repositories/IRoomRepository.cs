using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Repositories;

public interface IRoomRepository : IRepository<Room, Guid>
{
    Task<bool> ExistsByName(string name);
}