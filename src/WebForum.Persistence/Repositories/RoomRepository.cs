using Microsoft.EntityFrameworkCore;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class RoomRepository(ApplicationDbContext context) : GenericRepository<Room, Guid>(context), IRoomRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Room>().FirstOrDefaultAsync(x => x.Name == name, cancellationToken) != default;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Room>().FirstOrDefaultAsync(x => x.RoomId == id, cancellationToken) != default;
    }
}