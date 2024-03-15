using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Rooms.GetById;

public class GetRoomByIdQueryHandler(IRoomRepository roomRepository)
    : IQueryHandler<GetRoomByIdQuery, RoomResponse>
{
    public async Task<Result<RoomResponse>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        return room is null
            ? Result.Failure<RoomResponse>(DomainErrors.Room.NotFound(request.RoomId))
            : Result.Success(new RoomResponse(room.RoomId, room.Name, room.DateCreated, room.Description));
    }
}