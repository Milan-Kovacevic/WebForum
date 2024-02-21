using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Infrastructure.DbContext;

namespace WebForum.Infrastructure.Repositories;

public class TopicRepository(ApplicationDbContext context) : GenericRepository<Topic, Guid>(context), ITopicRepository
{

}