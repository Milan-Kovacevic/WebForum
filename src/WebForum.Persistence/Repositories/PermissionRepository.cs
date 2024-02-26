using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class PermissionRepository(ApplicationDbContext context) : GenericRepository<Permission, int>(context), IPermissionRepository
{
    
}