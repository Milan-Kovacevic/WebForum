using WebForum.Application.Abstractions.MediatR.Base;

namespace WebForum.Application.Features.Auth.Register;

public record RegisterCommand(string DisplayName, string Username, string Email, string Password) : ICommand
{
    public RequestFlag Type =>
        RequestFlag.Command | RequestFlag.Validate | RequestFlag.Transaction | RequestFlag.Sensitive;
};