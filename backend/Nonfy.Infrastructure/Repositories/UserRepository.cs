using Microsoft.EntityFrameworkCore;
using Nonfy.Domain.Entities;
using Nonfy.Domain.Exceptions;
using Nonfy.Domain.Repositories;

namespace Nonfy.Infrastructure.Repositories;

public class UserRepository(NonfyDbContext context) : IUserRepository
{
    private readonly NonfyDbContext _context = context;

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Add(user);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("Email") ?? false)
        {
            throw new DuplicateEmailException();
        }
    }
}
