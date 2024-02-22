namespace WebForum.Domain.Entities;

public class RegistrationRequest
{
    public Guid RequestId { get; init; }
    public Guid UserId { get; init; }
    public required DateTime SubmitDate { get; init; }
    public User? User { get; init; }
}