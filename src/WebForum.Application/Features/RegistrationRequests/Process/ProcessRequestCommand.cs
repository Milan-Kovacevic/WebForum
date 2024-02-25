using WebForum.Application.Abstractions.MediatR.Base;

namespace WebForum.Application.Features.RegistrationRequests.Process;

public record ProcessRequestCommand(Guid RequestId, bool ShouldAccept) : ICommand
{
    public RequestFlag Type => RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Sensitive | RequestFlag.Validate;
}