using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Features.Auth.Responses;

namespace WebForum.Application.Features.Auth.Login;

public record LoginCommand(string Username, string Password) : ICommand
{
    public virtual RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Validate | RequestFlag.Sensitive;
}