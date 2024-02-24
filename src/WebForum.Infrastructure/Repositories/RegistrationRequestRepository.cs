using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Infrastructure.DbContext;

namespace WebForum.Infrastructure.Repositories;

public class RegistrationRequestRepository(ApplicationDbContext context)
    : GenericRepository<RegistrationRequest, Guid>(context), IRegistrationRequestRepository
{
}