using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Rooms.Update;

public class UpdateRoomCommandHandler(IRoomRepository roomRepository)
    : ICommandHandler<UpdateRoomCommand, RoomResponse>
{
    public async Task<Result<RoomResponse>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room is null)
            return Result.Failure<RoomResponse>(DomainErrors.Room.NotFound(request.RoomId));

        room.Name = request.Name;
        room.Description = request.Description;
        roomRepository.Update(room);
        return Result.Success(new RoomResponse(room.RoomId, room.Name, room.DateCreated, room.Description));
    }
}