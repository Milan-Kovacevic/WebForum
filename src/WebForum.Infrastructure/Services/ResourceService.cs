using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;

namespace WebForum.Infrastructure.Services;

public class ResourceService(ICommentRepository commentRepository) : IResourceService
{
    public async Task<Guid?> FindRoomIdByCommentIdAsync(Guid commentId, CancellationToken cancellationToken = default)
    {
        var comment = await commentRepository.GetByIdAsync(commentId, cancellationToken);
        return comment?.RoomId;
    }
}