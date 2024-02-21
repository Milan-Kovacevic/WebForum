using WebForum.Application.Abstractions.Repositories;
using WebForum.Infrastructure.DbContext;

namespace WebForum.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}