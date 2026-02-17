using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces.Repository;
using ToDo.Infrastructure.Context;

namespace ToDo.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    #region IUserRepository Methods
    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteUserAsync(User user, CancellationToken cancellationToken)
    {
        context.Remove(user);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        context.Users.Update(user);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        => context.Users.FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);

    public Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        => context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> IsEmailRegisteredAsync(string email, CancellationToken cancellationToken)
        => context.Users.AnyAsync(u => u.Email.Value == email, cancellationToken);

    public Task<List<User>> GetUsersAsync(CancellationToken cancellationToken)
        => context.Users.AsNoTracking().ToListAsync(cancellationToken);
    #endregion
}