using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces.Repository;
using ToDo.Infrastructure.Context;

namespace ToDo.Infrastructure.Repositories;

public class TodoRepository(AppDbContext context) : ITodoRepository
{
    #region ITodoRepository Methods
    public async Task AddAsync(Todo task, CancellationToken cancellationToken)
    {
        await context.Todos.AddAsync(task, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(Todo task, CancellationToken cancellationToken)
    {
        context.Todos.Remove(task);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Todo task, CancellationToken cancellationToken)
    {
        context.Todos.Update(task);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAllAsync(IEnumerable<Todo> todos, CancellationToken cancellationToken)
    {
        context.Todos.RemoveRange(todos);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Todo?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken) 
        => await context.Todos.AsNoTracking().Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<Todo>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken) 
        => await context.Todos.AsNoTracking().Where(x => x.UserId == userId).ToListAsync(cancellationToken);
    #endregion
}