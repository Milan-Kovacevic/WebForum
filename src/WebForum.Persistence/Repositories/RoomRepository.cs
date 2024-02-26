using Microsoft.EntityFrameworkCore;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class RoomRepository(ApplicationDbContext context) : GenericRepository<Room, Guid>(context), IRoomRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> ExistsByName(string name)
    {
        return await _context.Set<Room>().FirstOrDefaultAsync(x => x.Name == name) != default;
    }
}