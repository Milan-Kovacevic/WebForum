using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Shared.Errors;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Rooms.Delete;

public class DeleteRoomCommandHandler(IRoomRepository roomRepository) : ICommandHandler<DeleteRoomCommand>
{
    public async Task<Result> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room is null)
            return Result.Failure(DomainErrors.Room.NotFound(request.RoomId));

        roomRepository.Delete(room);
        return Result.Success();
    }
}