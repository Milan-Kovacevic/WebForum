using System.Net;
using WebForum.Domain.Models.Errors;

namespace WebForum.Domain.Models.Results;

public partial class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
}

public class Result<TValue>(TValue? value, bool isSuccess, Error error) : Result(isSuccess, error)
{
    public TValue Value => IsSuccess
        ? value!
        : throw new InvalidOperationException("Failure value cannot be accessed");

    public static implicit operator Result<TValue>(TValue? value) => CreateFrom(value);
    public static Result<TValue> FromError(Error error) => new(default, false, error);
}

public partial class Result
{
    public static Result Success() => new(true, Error.None);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    
    public static Result<TValue> CreateFrom<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}