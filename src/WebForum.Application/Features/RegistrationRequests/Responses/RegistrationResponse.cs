namespace WebForum.Application.Features.RegistrationRequests.Responses;

public record RegistrationResponse(Guid RequestId, DateTime SubmitDate, string Username, string UserDisplayName);