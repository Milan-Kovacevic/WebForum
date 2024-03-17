using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Repositories;

public interface IPermissionRepository : IRepository<Permission, int>
{
}