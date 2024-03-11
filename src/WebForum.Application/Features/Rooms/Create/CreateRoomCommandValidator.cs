using FluentValidation;

namespace WebForum.Application.Features.Rooms.Create;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(96);
        RuleFor(x => x.Description).MaximumLength(512);
    }
}