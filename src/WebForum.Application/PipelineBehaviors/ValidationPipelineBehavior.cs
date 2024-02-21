using FluentValidation;
using MediatR;
using WebForum.Application.Abstractions.Messaging.MediatR;
using WebForum.Domain.Models.Errors;
using WebForum.Domain.Models.Results;

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
        var validationErrors = validationFailures.Where(result => !result.IsValid)
            .SelectMany(result => result.Errors).Select(validationFailure =>
                new Error(validationFailure.PropertyName, validationFailure.ErrorMessage)).ToArray();

        if (!validationErrors.Any())
            return await next();

        return CreateValidationResult<TResponse>(validationErrors);
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors) where TResult : Result
    {
        // Result with no response (non generic result object)
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        // Result with response type (generic result object)
        var validationResult = typeof(ValidationResult<>).GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!.Invoke(null, [errors])!;

        return (TResult)validationResult;
    }
}