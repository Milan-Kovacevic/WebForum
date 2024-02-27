using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Comments.Edit;

public class EditCommentCommandHandler(ICommentRepository commentRepository)
    : ICommandHandler<EditCommentCommand, CommentResponse>
{
    public async Task<Result<CommentResponse>> Handle(EditCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment is null)
            return Result.Failure<CommentResponse>(DomainErrors.Comment.NotFound(request.CommentId));
        if (comment.Status != CommentStatus.Created)
            return Result.Failure<CommentResponse>(DomainErrors.Comment.NotEditable(request.CommentId));

        comment.Content = request.NewContent;
        comment.DateUpdated = DateTime.UtcNow;
        commentRepository.Update(comment);
        
        return Result.Success(new CommentResponse(comment.CommentId, comment.Content, comment.DateCreated,
            comment.DateUpdated, comment.DatePosted, comment.User.DisplayName, comment.User.Role!.Name));
    }
}