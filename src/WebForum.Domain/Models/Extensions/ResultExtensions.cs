using WebForum.Domain.Models.Results;

namespace WebForum.Domain.Models.Extensions;

public static class ResultExtensions
{
    public static async Task<Result> Bind<TIn>(this Result<TIn> result, Func<TIn, Task<Result>> func)
    {

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return result;
    }
}