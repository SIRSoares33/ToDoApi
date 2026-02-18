using MediatR;
using ToDo.Application.DTOs;
using ToDo.Domain.Entities;

namespace ToDo.Application.Interfaces;

/// <summary>
/// this service interface defines the contract for user-related operations in the application, 
/// including retrieving all users, deleting a user by their unique identifier,
/// and updating a user's information based on their unique identifier and provided data transfer object (DTO). 
/// The methods are asynchronous and support cancellation through the use of CancellationToken parameters. 
/// Implementations of this interface will handle the actual logic for interacting with the underlying data store to perform these operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// gets a list of all users in the system.
    /// The method is asynchronous and returns a Task that, 
    /// when completed, will provide a List of User entities.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken);
    /// <summary>
    /// deletes a user from the system based on their unique identifier (userId).
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Unit> DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
    /// <summary>
    /// updates a user's information in the system based on their unique identifier (id)
    /// and the provided UpdateUserDto.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Unit> UpdateUserAsync(Guid id, UpdateUserDto dto, CancellationToken cancellationToken);
}