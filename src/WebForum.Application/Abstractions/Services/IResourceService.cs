using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Services;

public interface IResourceService
{
    Task<Guid?> FindRoomIdByCommentIdAsync(Guid commentId, CancellationToken cancellationToken = default);
}