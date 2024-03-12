using WebForum.Domain.Entities;
using WebForum.Domain.Enums;

namespace WebForum.Application.Abstractions.Repositories;

public interface ICommentRepository : IRepository<Comment, Guid>
{
    Task<IEnumerable<Comment>> GetPostedRoomCommentsForUserAsync(Guid roomId, Guid userId, int limit,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Comment>> GetRoomCommentsByStatusLimitedAsync(Guid roomId, CommentStatus status, int limit,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Comment>> GetRoomCommentsByStatusAsync(Guid roomId, CommentStatus status,
        CancellationToken cancellationToken = default);
}