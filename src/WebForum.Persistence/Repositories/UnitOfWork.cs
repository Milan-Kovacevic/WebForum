using WebForum.Application.Abstractions.Repositories;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}