using MediatR;
using ToDo.Application.DTOs;
using ToDo.Domain.Entities;

namespace ToDo.Application.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<Unit> DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<Unit> UpdateUserAsync(Guid id, User userInput, CancellationToken cancellationToken);
}