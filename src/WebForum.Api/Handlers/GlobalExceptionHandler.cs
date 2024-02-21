
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebForum.Api.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception occured on {@Source}: {Message}", exception.Source, exception.Message);
        var exceptionDetails = GetExceptionDetails(exception);

        var problemDetails = new ProblemDetails()
        {
            Status = exceptionDetails.Status,
            Title = exceptionDetails.Title,
            Type = exceptionDetails.Type,
            Detail = exceptionDetails.Detail
        };
        if (exceptionDetails.Errors is not null)
        {
            problemDetails.Extensions["errors"] = exceptionDetails.Errors;
        }

        httpContext.Response.StatusCode = exceptionDetails.Status;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return new ExceptionDetails(StatusCodes.Status500InternalServerError, "Server Error", "Internal Server Error",
            "An unexpected error has occured", null);
    }

    private sealed record ExceptionDetails(
        int Status,
        string Title,
        string Type,
        string Detail,
        IEnumerable<object>? Errors);
}