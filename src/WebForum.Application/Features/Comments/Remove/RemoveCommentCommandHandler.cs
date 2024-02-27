using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Comments.Remove;

public class RemoveCommentCommandHandler(ICommentRepository commentRepository) : ICommandHandler<RemoveCommentCommand>
{
    public async Task<Result> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment is null)
            return Result.Failure(DomainErrors.Comment.NotFound(request.CommentId));
        
        commentRepository.Delete(comment);
        return Result.Success();
    }
}