using FluentValidation;
using MediatR;
using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.PipelineBehaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>>? validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequestType
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!request.Type.HasFlag(RequestFlag.Validate))
            return await next();
        if (validators is null || !validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationFailures =
            await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));
        var propertyErrors = validationFailures.Where(result => !result.IsValid)
            .SelectMany(result => result.Errors).Select(validationFailure =>
                new PropertyError(validationFailure.PropertyName, validationFailure.ErrorMessage)).ToArray();

        if (!propertyErrors.Any())
            return await next();

        return CreateValidationResult<TResponse>(propertyErrors);
    }

    private static TResult CreateValidationResult<TResult>(PropertyError[] errors) where TResult : Result
    {
        // Result with no response (non generic result object)
        if (typeof(TResult) == typeof(Result))
        {
            return (Result.Failure(new ValidationError(errors)) as TResult)!;
        }
        
        // Result with a response (generic result object)
        var validationResult = typeof(Result<>).GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(Result<TResult>.FromError))!.Invoke(null, [new ValidationError(errors)])!;
        return (TResult)validationResult;
    }
}