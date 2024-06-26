namespace WebForum.Domain.Entities;

public class Room
{
    public Guid RoomId { get; init; }
    public required string Name { get; set; }
    public required DateTime DateCreated { get; set; }
    public string? Description { get; set; }
}