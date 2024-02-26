using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Features.Topics.Responses;

namespace WebForum.Application.Features.Rooms.GetById;

public record GetRoomByIdQuery(Guid RoomId) : IQuery<RoomResponse>
{
    public RequestFlag Type => RequestFlag.Query;
}