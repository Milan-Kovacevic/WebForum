using MediatR;

namespace WebForum.Application.Abstractions.Messaging.MediatR;

public interface IRequestType
{
    public RequestFlag Type { get; }
}

public interface IRequestBase : IRequest, IRequestType;

public interface IRequestBase<out TResponse> : IRequest<TResponse>, IRequestType;

[Flags]
public enum RequestFlag
{
    None = 0,
    Command = 1,
    Query = 2,
    Transaction = 4,
    Validate = 8,
    Sensitive = 16,
    All = int.MaxValue
}