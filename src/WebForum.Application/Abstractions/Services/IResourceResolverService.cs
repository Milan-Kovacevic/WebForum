namespace WebForum.Application.Abstractions.Services;

public interface IResourceResolverService
{
    Task<Guid?> ResolveRoomIdByCommentIdAsync(Guid commentId, CancellationToken cancellationToken = default);
}