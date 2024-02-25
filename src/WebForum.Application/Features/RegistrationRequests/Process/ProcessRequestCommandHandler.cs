using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Abstractions.Services;
using WebForum.Application.Models;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.RegistrationRequests.Process;

public class ProcessRequestCommandHandler(
    IRegistrationRequestRepository registrationRequestRepository,
    IUserRepository userRepository,
    IEmailService emailService,
    IUnitOfWork unitOfWork) : ICommandHandler<ProcessRequestCommand>
{
    public async Task<Result> Handle(ProcessRequestCommand request, CancellationToken cancellationToken)
    {
        var registrationRequest =
            await registrationRequestRepository.GetByIdAsync(request.RequestId, cancellationToken);

        if (registrationRequest is null)
            return Result.Failure(DomainErrors.RegistrationRequest.NotFound(request.RequestId));

        if (request.ShouldAccept)
            registrationRequest.User.IsEnabled = true;
        else 
            userRepository.Delete(registrationRequest.User);
        
        registrationRequestRepository.Delete(registrationRequest);
        var emailMessage = new SimpleEmail()
        {
            SendTo = registrationRequest.User.Email!,
            Body = $"Your registration request for our application has been {(request.ShouldAccept ? "accepted" : "rejected")}.",
            Subject = "Processing of registration request is completed"
        };
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await emailService.SendSimpleEmail(emailMessage, cancellationToken);
        return Result.Success();
    }
}