namespace WebForum.Domain.Entities;

public class TwoFactorCode {
    public required Guid UserId { get; init; }
    public required string Value { get; init; }
    public required TimeSpan Duration { get; init; }
}