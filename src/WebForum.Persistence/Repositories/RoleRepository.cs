using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class RoleRepository(ApplicationDbContext context) : GenericRepository<UserRole, int>(context), IRoleRepository
{
}