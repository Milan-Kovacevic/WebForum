using MediatR;
using WebForum.Domain.Models.Results;

namespace WebForum.Application.Abstractions.MediatR.Base;

// Marker interfaces
public interface IQuery<TResponse> : IRequestBase<Result<TResponse>>;
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;