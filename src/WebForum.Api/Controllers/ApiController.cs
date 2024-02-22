using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebForum.Domain.Models.Errors;
using WebForum.Domain.Models.Results;

namespace WebForum.Api.Controllers;

[ApiController]
public class ApiController(ISender sender) : ControllerBase
{
    protected ISender Sender { get; } = sender;
    
    protected IActionResult HandleFailure(Result result)
        => result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult => BadRequest(GetProblemDetails(result.Error, validationResult.Errors)),
            { Error.Status: StatusCodes.Status404NotFound } => NotFound(GetProblemDetails(result.Error)),
            { Error.Status: StatusCodes.Status401Unauthorized } => Unauthorized(GetProblemDetails(result.Error)),
            { Error.Status: StatusCodes.Status403Forbidden } => Forbid(),
            { Error.Status: StatusCodes.Status409Conflict } => Conflict(GetProblemDetails(result.Error)),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };

    private static ProblemDetails GetProblemDetails(Error error, Error[]? validationErrors = null)
    {
        var problemDetails = new ProblemDetails()
        {
            Status = error.Status,
            Title = error.Code,
            Type = error.Code,
            Detail = error.Message
        };
        if (validationErrors is not null)
        {
            problemDetails.Extensions["errors"] = validationErrors;
        }

        return problemDetails;
    }
}