using WebForum.Domain.Models.Results;

namespace WebForum.Domain.Models.Extensions;

public static class ResultExtensions
{
    public static async Task<Result> Process<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> func)
    {
        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return await func(result.Value);
    }
    
    public static async Task<Result<TOut>> Process<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> func)
    {
        if (result.IsFailure)
        {
            return Result.Failure<TOut>(result.Error);
        }

        return await func(result.Value);
    }
}