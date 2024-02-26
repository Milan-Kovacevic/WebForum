using Microsoft.EntityFrameworkCore;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : GenericRepository<User, Guid>(context), IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> ExistsByUsername(string username)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(x => x.Username == username) != default;
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await _context.Set<User>()
            .FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<IEnumerable<User>> GetAllRegisteredAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>()
            .AsNoTracking()
            .Where(x => x.RegistrationRequest == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdWithPermissionsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>()
            .Include(x => x.Permissions)
            .ThenInclude(p => p.Permission)
            .FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
    }
}