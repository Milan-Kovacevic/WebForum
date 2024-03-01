using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Utils;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;
using UserRole = WebForum.Domain.Entities.UserRole;

namespace WebForum.Application.Features.Auth.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IAuthService authService,
    IRegistrationRequestRepository registrationRequestRepository) : ICommandHandler<RegisterCommand>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByUsernameAsync(request.Username, cancellationToken))
            return Result.Failure(DomainErrors.User.ConflictUsername(request.Username));

        var user = new User()
        {
            DisplayName = request.DisplayName,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = authService.ComputePasswordHash(request.Password),
            AccessFailedCount = Constants.DefaultLoginAccessFailCount,
            IsEnabled = false,
            RoleId = UserRole.Regular.RoleId
        };
        await userRepository.InsertAsync(user, cancellationToken);

        var registrationRequest = new RegistrationRequest()
        {
            UserId = user.UserId,
            User = user,
            SubmitDate = DateTime.UtcNow,
        };
        await registrationRequestRepository.InsertAsync(registrationRequest, cancellationToken);
        return Result.Success();
    }
}