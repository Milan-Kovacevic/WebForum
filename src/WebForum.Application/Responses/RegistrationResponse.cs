namespace WebForum.Application.Responses;

public record RegistrationResponse(Guid RequestId, DateTime SubmitDate, string Username, string UserDisplayName, string? UserEmail);