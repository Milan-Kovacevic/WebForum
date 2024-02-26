using WebForum.Application.Abstractions.MediatR.Base;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Application.Features.Topics.Responses;
using WebForum.Domain.Shared.Results;

namespace WebForum.Application.Features.Rooms.GetAll;

public class GetAllRoomsQueryHandler(IRoomRepository roomRepository)
    : IQueryHandler<GetAllRoomsQuery, IEnumerable<RoomResponse>>
{
    public async Task<Result<IEnumerable<RoomResponse>>> Handle(GetAllRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var rooms = await roomRepository.GetAllAsync(cancellationToken);
        return Result.Success(rooms.Select(x => new RoomResponse(x.RoomId, x.Name, x.Description)));
    }
}