using Microsoft.EntityFrameworkCore;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Persistence.Configuration;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class CommentRepository(ApplicationDbContext context)
    : GenericRepository<Comment, Guid>(context), ICommentRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Comment>> GetPostedRoomCommentsForUserAsync(Guid roomId, Guid userId, int limit, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Comment>()
            .AsNoTracking()
            .Include(x => x.User)
            .ThenInclude(u => u.Role)
            .Where(x => (x.RoomId == roomId && x.Status == CommentStatus.Posted) 
                        || (x.RoomId == roomId && x.Status == CommentStatus.Created && x.UserId == userId))
            .OrderByDescending(x => x.DateCreated)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetRoomCommentsByStatusLimitedAsync(Guid roomId, CommentStatus status,
        int limit = Database.Constants.MaxLatestPostedComments,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Comment>()
            .AsNoTracking()
            .Include(x => x.User)
            .ThenInclude(u => u.Role)
            .Where(x => x.RoomId == roomId && x.Status == status)
            .OrderByDescending(x => x.DateCreated)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetRoomCommentsByStatusAsync(Guid roomId, CommentStatus status,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Comment>()
            .AsNoTracking()
            .Include(x => x.User)
            .ThenInclude(u => u.Role)
            .Where(x => x.RoomId == roomId && x.Status == status)
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Comment>()
            .Include(x => x.User)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(x => x.CommentId == id, cancellationToken: cancellationToken);
    }
}