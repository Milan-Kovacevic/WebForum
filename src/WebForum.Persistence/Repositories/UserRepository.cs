using Microsoft.EntityFrameworkCore;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : GenericRepository<User, Guid>(context), IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(x => x.Username == username, cancellationToken) !=
               default;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(x => x.UserId == id, cancellationToken) != default;
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>()
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllRegisteredAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>()
            .AsNoTracking()
            .Include(x => x.Role)
            .Where(x => x.RegistrationRequest == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdWithPermissionsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>()
            .Include(x => x.Role)
            .Include(x => x.Permissions).ThenInclude(p => p.Permission)
            .Include(x => x.Permissions).ThenInclude(p => p.Room)
            .FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
    }
}