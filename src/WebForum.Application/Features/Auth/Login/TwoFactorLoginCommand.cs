using WebForum.Application.Abstractions.MediatR.Base;

namespace WebForum.Application.Features.Auth.Login;

public record TwoFactorLoginCommand(string Username, string Password, string TwoFactorCode)
    : LoginCommand(Username, Password)
{
    public override RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Transaction | RequestFlag.Validate | RequestFlag.Sensitive;
}