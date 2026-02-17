using ToDo.Domain.Entities;

namespace ToDo.Domain.Interfaces.Repository;

/// <summary>
/// Defines a contract for managing user data, including retrieval, creation, update, and deletion operations.
/// </summary>
/// <remarks>Implementations of this interface provide asynchronous methods for accessing and modifying user
/// information in a data store. All operations are performed asynchronously to support scalable and responsive
/// applications. Methods that accept parameters require non-null values unless otherwise specified.</remarks>
public interface IUserRepository
{
    /// <summary>
    /// Asynchronously determines whether the specified email address is registered.
    /// </summary>
    /// <param name="email">The email address to check for registration. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the email
    /// address is registered; otherwise, <see langword="false"/>.</returns>
    Task<bool> IsEmailRegisteredAsync(string email, CancellationToken cancellationToken);
    /// <summary>
    /// Asynchronously retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user associated with the
    /// specified email address, or null if no such user exists.</returns>
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    /// <summary>
    /// Asynchronously retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user associated with the
    /// specified identifier, or null if no such user exists.</returns>
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Asynchronously adds a new user to the system.
    /// </summary>
    /// <param name="user">The user information to add. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task AddUserAsync(User user, CancellationToken cancellationToken);
    /// <summary>
    /// Asynchronously updates the specified user's information in the data store.
    /// </summary>
    /// <param name="user">The user model containing updated information for the user. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateUserAsync(User user, CancellationToken cancellationToken);
    /// <summary>
    /// Asynchronously deletes the specified user from the system.
    /// </summary>
    /// <param name="user">The user to delete. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteUserAsync(User user, CancellationToken cancellationToken);
    /// <summary>
    /// Asynchronously retrieves a list of all users in the system.
    /// </summary>
    /// <returns></returns>
    Task<List<User>> GetUsersAsync(CancellationToken cancellationToken);
}
