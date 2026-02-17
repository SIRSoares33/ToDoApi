using MediatR;

namespace ToDo.Application.Features.Auth.Commands;

/// <summary>
/// Represents a request to delete a user identified by a unique identifier.
/// </summary>
/// <param name="UserId">The unique identifier of the user to be deleted.</param>
public record DeleteUserCommand(Guid UserId) : IRequest<Unit>;