using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Features.Auth.Responses;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.Features.Auth.Login;

public class TwoFactorLoginCommandHandler : ICommandHandler<TwoFactorLoginCommand, LoginResponse>
{
    public Task<Result<LoginResponse>> Handle(TwoFactorLoginCommand request, CancellationToken cancellationToken)
    {
        
        
        
        throw new NotImplementedException();
    }
}