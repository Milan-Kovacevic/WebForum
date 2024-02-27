using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllRegisteredAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdWithPermissionsAsync(Guid id, CancellationToken cancellationToken = default);
}