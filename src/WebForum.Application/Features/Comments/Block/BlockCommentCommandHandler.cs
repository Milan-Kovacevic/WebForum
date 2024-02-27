using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Comments.Block;

public class BlockCommentCommandHandler(ICommentRepository commentRepository) : ICommandHandler<BlockCommentCommand>
{
    public async Task<Result> Handle(BlockCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment is null)
            return Result.Failure(DomainErrors.Comment.NotFound(request.CommentId));

        comment.Status = CommentStatus.Banned;
        commentRepository.Update(comment);
        return Result.Success();
    }
}