using MediatR;
using ToDo.Application.DTOs;

namespace ToDo.Application.Interfaces;

public interface ITodoService
{
    Task<Unit> CreateTodoAsync(AddToDoDto todo, CancellationToken cancellationToken);
    Task<Unit> DeleteTodoAsync(Guid id, CancellationToken cancellationToken);
    Task<Unit> DeleteAllTodosAsync(CancellationToken cancellationToken);
    Task<List<ToDoDto>> GetAllTodosAsync(CancellationToken cancellationToken);
    Task<ToDoDto> GetTodoByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Unit> UpdateTodoAsync(Guid id, UpdateToDoDto updatedTodo, CancellationToken cancellationToken);
}