using MediatR;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Commands.Users;
/// <summary>
/// Command to update a user's information. 
/// This command is intended for users to update their own account details, 
/// such as name, email, or password. It does not allow changing the user's role,
/// and any attempt to do so will result in an unauthorized access exception. 
/// The command takes the user's unique identifier (id) and an UpdateUserDto containing the new information to be updated. 
/// The handler for this command will perform the necessary validation and update operations on the user entity in the database.
/// </summary>
/// <param name="id"></param>
/// <param name="dto"></param>
public record UpdateUserCommand(Guid id, UpdateUserDto dto) : IRequest<Unit>;