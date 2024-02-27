using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Responses;
using WebForum.Domain.Entities;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Rooms.Create;

public class CreateRoomCommandHandler(IRoomRepository roomRepository)
    : ICommandHandler<CreateRoomCommand, RoomResponse>
{
    public async Task<Result<RoomResponse>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        if (await roomRepository.ExistsByNameAsync(request.Name, cancellationToken))
            return Result.Failure<RoomResponse>(DomainErrors.Room.ConflictName(request.Name));

        var room = new Room()
        {
            Name = request.Name,
            Description = request.Description
        };
        await roomRepository.InsertAsync(room, cancellationToken);
        return Result.Success(new RoomResponse(room.RoomId, room.Name, room.Description));
    }
}