using WebForum.Domain.Entities;
using WebForum.Domain.Enums;

namespace WebForum.Application.Abstractions.Repositories;

public interface IUserLoginRepository
{
    Task InsertAsync(UserLogin entity, CancellationToken cancellationToken = default);

    Task<bool> ExistsByProviderAsync(LoginProvider loginProvider, string providerKey,
        CancellationToken cancellationToken = default);

    Task<UserLogin?> GetByProviderAsync(LoginProvider loginProvider, string providerKey,
        CancellationToken cancellationToken = default);

    void Delete(UserLogin entity);
}