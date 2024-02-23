using Microsoft.AspNetCore.Mvc;
using WebForum.Domain.Models.Results;

namespace WebForum.Api.Configuration.Extensions;

public static class ResultExtensions
{
    public static async Task<IActionResult> Respond(this Task<Result> resultTask, Func<IActionResult> onSuccess,
        Func<Result, IActionResult> onFailure)
    {
        var result = await resultTask;
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    public static async Task<IActionResult> Respond<TResponse>(this Task<Result<TResponse>> resultTask,
        Func<TResponse, IActionResult> onSuccess,
        Func<Result, IActionResult> onFailure)
    {
        var result = await resultTask;
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}