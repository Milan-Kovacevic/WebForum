using WebForum.Domain.Models.Errors;

namespace WebForum.Domain.Models.Results;

public class ValidationResult(Error[] errors) 
    : Result(false, Error.ValidationError), IValidationResult
{
    public Error[]? Errors { get; } = errors;

    public static ValidationResult WithErrors(Error[] validationErrors) => new(validationErrors);
}

public class ValidationResult<TValue>(Error[] errors)
    : Result<TValue>(default, false, Error.ValidationError), IValidationResult
{
    public Error[]? Errors { get; } = errors;
    public static ValidationResult<TValue> WithErrors(Error[] validationErrors) => new(validationErrors);
}

public interface IValidationResult
{
    public Error[]? Errors { get; }
}