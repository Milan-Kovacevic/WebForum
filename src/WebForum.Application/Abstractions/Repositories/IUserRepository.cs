using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<bool> ExistsByUsername(string username);
    Task<User?> GetByUsername(string username);
}