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
            IValidationResult validationResult => BadRequest(GetProblemDetails(StatusCodes.Status400BadRequest,
                result.Error.Name, result.Error, validationResult.Errors)),
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };

    private static ProblemDetails GetProblemDetails(int status, string title, Error error, Error[]? validationErrors)
    {
        var problemDetails = new ProblemDetails()
        {
            Status = status,
            Title = title,
            Type = error.Name,
            Detail = error.Message
        };
        if (validationErrors is not null)
        {
            problemDetails.Extensions["errors"] = validationErrors;
        }

        return problemDetails;
    }
}