namespace WebForum.Domain.Models.Errors;

public record Error(string Name, string Message)
{
    public static readonly Error None = new Error(string.Empty, string.Empty);
    public static readonly Error NullValue = new Error($"Error.NullValue", "The specified result value is null.");
    public static readonly Error ConditionNotMet = new("Error.ConditionNotMet", "The specified condition was not met.");
    public static readonly Error ValidationError = new Error("Error.Validation", "A validation problem occured.");

    public static implicit operator string(Error error) => error.Name;
}