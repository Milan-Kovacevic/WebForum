using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Auth.Login;

public record TwoFactorLoginCommand(string Username, string Password, string TwoFactorCode)
    : ICommand<LoginResponse>
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Validate | RequestFlag.Sensitive;
}