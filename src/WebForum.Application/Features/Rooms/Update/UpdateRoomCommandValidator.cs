using FluentValidation;

namespace WebForum.Application.Features.Rooms.Update;

public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Description).MaximumLength(128);
    }
}