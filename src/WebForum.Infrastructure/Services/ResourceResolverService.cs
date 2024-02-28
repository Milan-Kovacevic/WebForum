using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;

namespace WebForum.Infrastructure.Services;

public class ResourceResolverService(ICommentRepository commentRepository) : IResourceResolverService
{
    public async Task<Guid?> ResolveRoomIdByCommentIdAsync(Guid commentId, CancellationToken cancellationToken = default)
    {
        var comment = await commentRepository.GetByIdAsync(commentId, cancellationToken);
        return comment?.RoomId;
    }
}