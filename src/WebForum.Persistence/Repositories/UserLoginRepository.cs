using Microsoft.EntityFrameworkCore;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Domain.Enums;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class UserLoginRepository(ApplicationDbContext context) : IUserLoginRepository
{
    public async Task InsertAsync(UserLogin entity, CancellationToken cancellationToken = default)
    {
        await context.Set<UserLogin>().AddAsync(entity, cancellationToken);
    }

    public async Task<bool> ExistsByProviderAsync(LoginProvider loginProvider, string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<UserLogin>().FirstOrDefaultAsync(
            x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey, cancellationToken) != default;
    }

    public async Task<UserLogin?> GetByProviderAsync(LoginProvider loginProvider, string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<UserLogin>()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey,
                cancellationToken);
    }

    public void Delete(UserLogin entity)
    {
        context.Set<UserLogin>().Remove(entity);
    }
}