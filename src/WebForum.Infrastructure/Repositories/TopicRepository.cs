using WebForum.Domain.Entities;
using WebForum.Domain.Interfaces;
using WebForum.Infrastructure.DbContext;

namespace WebForum.Infrastructure.Repositories;

public class TopicRepository(ApplicationDbContext context) : GenericRepository<Topic, Guid>(context), ITopicRepository
{

}