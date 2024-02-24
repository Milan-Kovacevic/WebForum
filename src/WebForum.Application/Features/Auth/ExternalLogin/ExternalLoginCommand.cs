using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Features.Auth.Responses;

namespace WebForum.Application.Features.Auth.ExternalLogin;

public record ExternalLoginCommand(string Code, string Provider) : ICommand<LoginResponse>
{
    public RequestFlag Type => RequestFlag.Command | RequestFlag.Validate | RequestFlag.Sensitive;
}