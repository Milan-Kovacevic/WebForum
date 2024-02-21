using MediatR;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.Abstractions.Messaging.MediatR;


// Marker interfaces
public interface ICommand : IRequestBase<Result>;
public interface ICommand<TResponse> : IRequestBase<Result<TResponse>>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand;
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>;