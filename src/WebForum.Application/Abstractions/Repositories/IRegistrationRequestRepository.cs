using WebForum.Domain.Entities;

namespace WebForum.Application.Abstractions.Repositories;

public interface IRegistrationRequestRepository : IRepository<RegistrationRequest, Guid>
{
    
}