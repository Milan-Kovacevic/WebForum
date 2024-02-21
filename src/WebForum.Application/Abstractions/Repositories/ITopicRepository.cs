using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Repositories;

public interface ITopicRepository : IRepository<Topic, Guid>
{
}