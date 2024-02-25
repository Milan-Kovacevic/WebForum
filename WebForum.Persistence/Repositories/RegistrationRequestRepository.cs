using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class RegistrationRequestRepository(ApplicationDbContext context)
    : GenericRepository<RegistrationRequest, Guid>(context), IRegistrationRequestRepository
{
}