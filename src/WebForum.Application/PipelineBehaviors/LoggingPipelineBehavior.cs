using MediatR;
using Microsoft.Extensions.Logging;
using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.PipelineBehaviors;

public class LoggingPipelineBehavior<TRequest, TResponse>(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequestBase<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request.Type.HasFlag(RequestFlag.Sensitive))
        {
            logger.LogInformation("Starting sensitive request {@RequestName}, at {@DateTimeUtc}", typeof(TRequest).Name,
                DateTime.UtcNow);
        }
        else
        {
            logger.LogDebug("Starting request {@RequestName}, at {@DateTimeUtc}", typeof(TRequest).Name,
                DateTime.UtcNow);
        }

        var result = await next();

        if (result.IsFailure)
        {
            LogFailure(result);
            return result;
        }

        if (request.Type.HasFlag(RequestFlag.Sensitive))
        {
            logger.LogInformation("Completed sensitive request {@RequestName}, at {@DateTimeUtc}",
                typeof(TRequest).Name,
                DateTime.UtcNow);
        }
        else
        {
            logger.LogDebug("Completed request {@RequestName}, {@DateTimeUtc}", typeof(TRequest).Name,
                DateTime.UtcNow);
        }

        return result;
    }

    private void LogFailure(TResponse result)
    {
        if (!result.IsFailure)
            return;

        if (result is IValidationResult validationResult)
        {
            logger.LogError(
                "Request failure {@RequestName}, {@Error}, {@DateTimeUtc}. Validation errors {@ValidationErrors}",
                typeof(TRequest).Name,
                validationResult.Errors, DateTime.UtcNow, validationResult.Errors);
        }
        else
        {
            logger.LogError("Request failure {@RequestName}, {@Error}, {@DateTimeUtc}", typeof(TRequest).Name,
                result.Error, DateTime.UtcNow);
        }
    }
}