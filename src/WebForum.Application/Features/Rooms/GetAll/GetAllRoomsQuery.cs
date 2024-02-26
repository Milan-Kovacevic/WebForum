using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Responses;

namespace WebForum.Application.Features.Rooms.GetAll;

public record GetAllRoomsQuery : IQuery<IEnumerable<RoomResponse>>
{
    public RequestFlag Type => RequestFlag.Query;
}