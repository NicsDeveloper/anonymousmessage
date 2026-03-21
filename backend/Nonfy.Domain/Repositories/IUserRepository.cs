using Nonfy.Domain.Entities;

namespace Nonfy.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
}
