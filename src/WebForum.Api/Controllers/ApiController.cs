using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Api.Controllers;

[ApiController]
[Authorize]
public class ApiController(ISender sender) : ControllerBase
{
    protected ISender Sender { get; } = sender;

    protected IActionResult HandleFailure(Result result)
        => result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            { Error.Status: StatusCodes.Status401Unauthorized } => Unauthorized(GetProblemDetails(result.Error)),
            { Error.Status: StatusCodes.Status403Forbidden } => Forbid(),
            { Error.Status: StatusCodes.Status404NotFound } => NotFound(GetProblemDetails(result.Error)),
            { Error.Status: StatusCodes.Status409Conflict } => Conflict(GetProblemDetails(result.Error)),
            { Error: { Status: StatusCodes.Status400BadRequest } and ValidationError validationError } =>
                BadRequest(GetProblemDetails(result.Error, validationError.Errors)),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };

    private static ProblemDetails GetProblemDetails(Error error, PropertyError[]? propertyErrors = null)
    {
        var problemDetails = new ProblemDetails()
        {
            Status = error.Status,
            Title = error.Code,
            Type = error.Code,
            Detail = error.Message
        };
        if (propertyErrors is not null)
        {
            problemDetails.Extensions["errors"] = propertyErrors;
        }

        return problemDetails;
    }
}