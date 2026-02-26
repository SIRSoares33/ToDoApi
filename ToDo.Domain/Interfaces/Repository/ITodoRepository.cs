using ToDo.Domain.Entities;

namespace ToDo.Domain.Interfaces.Repository;

/// <summary>
/// Defines the data access and manipulation methods for the <see cref="Todo"/> entity.
/// </summary>
public interface ITodoRepository
{
    /// <summary>
    /// Adds a new task to the repository asynchronously.
    /// </summary>
    /// <param name="task">The task to add.</param>
    Task AddAsync(Todo task, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all tasks for a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The identifier of the user whose tasks will be returned.</param>
    /// <returns>A list of tasks associated with the user.</returns>
    Task<List<Todo>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a task by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the task.</param>
    /// <returns>The corresponding task, or <c>null</c> if not found.</returns>
    Task<Todo?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes an existing task from the repository asynchronously.
    /// </summary>
    /// <param name="task">The task to remove.</param>
    Task RemoveAsync(Todo task, CancellationToken cancellationToken);
    /// <summary>
    /// Remove all user todos.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveAllAsync(IEnumerable<Todo> todos, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the data of an existing task asynchronously.
    /// </summary>
    /// <param name="task">The task with updated data.</param>
    Task UpdateAsync(Todo task, CancellationToken cancellationToken);
}