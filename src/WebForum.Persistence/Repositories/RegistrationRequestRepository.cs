using Microsoft.EntityFrameworkCore;
using WebForum.Application.Abstractions.Repositories;
using WebForum.Domain.Entities;
using WebForum.Persistence.DbContext;

namespace WebForum.Persistence.Repositories;

public class RegistrationRequestRepository(ApplicationDbContext context)
    : GenericRepository<RegistrationRequest, Guid>(context), IRegistrationRequestRepository
{
    private readonly ApplicationDbContext _context = context;

    public override async Task<IEnumerable<RegistrationRequest>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<RegistrationRequest>()
            .AsNoTracking()
            .Include(x => x.User)
            .ToListAsync(cancellationToken);
    }

    public override async Task<RegistrationRequest?> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<RegistrationRequest>()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.RequestId == id, cancellationToken);
    }
}