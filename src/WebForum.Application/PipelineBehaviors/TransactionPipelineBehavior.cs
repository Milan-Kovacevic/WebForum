using System.Transactions;
using MediatR;
using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.PipelineBehaviors;

public class TransactionPipelineBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequestType
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request.Type.HasFlag(RequestFlag.Transaction))
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var response = await next();
            if (response.IsFailure)
                return response;

            await unitOfWork.SaveChangesAsync(cancellationToken);
            transactionScope.Complete();
            return response;
        }

        if (!request.Type.HasFlag(RequestFlag.Command))
            return await next();

        var result = await next();
        if (result.IsFailure)
            return result;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}