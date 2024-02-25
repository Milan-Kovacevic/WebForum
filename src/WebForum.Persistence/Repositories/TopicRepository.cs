using Microsoft.EntityFrameworkCore;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class TopicRepository(ApplicationDbContext context) : GenericRepository<Topic, Guid>(context), ITopicRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> ExistsByName(string name)
    {
        return await _context.Set<Topic>().FirstOrDefaultAsync(x => x.Name == name) != default;
    }
}