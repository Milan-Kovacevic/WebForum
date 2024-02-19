using WebForum.Domain.Entities;

namespace WebForum.Domain.Interfaces;

public interface ITopicRepository : IRepository<Topic, Guid>
{
}