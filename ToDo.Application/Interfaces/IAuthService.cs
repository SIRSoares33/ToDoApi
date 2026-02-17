using MediatR;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Auth.Responses;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;

namespace ToDo.Application.Interfaces;

/// <summary>
/// Defines methods for user authentication and registration operations.
/// </summary>
/// <remarks>Implementations of this interface provide asynchronous authentication functionality, including user
/// login and registration. Methods return tasks that complete when the operation finishes, allowing for non-blocking
/// usage in client applications.</remarks>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user asynchronously using the provided login credentials.
    /// </summary>
    /// <param name="dto">An object containing the user's login information, such as username and password. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="LoginResponse"/>
    /// indicating the outcome of the authentication attempt.</returns>
    Task<LoginResponse> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
    /// <summary>
    /// Registers a new user asynchronously with the specified role.
    /// </summary>
    /// <param name="dto">The registration data for the new user. Cannot be null.</param>
    /// <param name="role">The role to assign to the user during registration.</param>
    /// <returns>A task that represents the asynchronous registration operation. The task result contains a unit value when the
    /// registration completes.</returns>
    Task<Unit> RegisterAsync(User user, Role role, CancellationToken cancellationToken);
}