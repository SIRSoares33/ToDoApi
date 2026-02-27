using AutoMapper;
using MediatR;
using ToDo.Application.DTOs;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces.Repository;

namespace ToDo.Infrastructure.Services;

public class TodoService(ITodoRepository repository, IMapper mapper, IUserClaims userClaims) : ITodoService
{
    #region ITotoService implementation
    public async Task<Unit> CreateTodoAsync(AddToDoDto dto, CancellationToken cancellationToken)
    {
        var mappedTodo = mapper.Map<Todo>(dto);

        mappedTodo.UserId = userClaims.UserId;

        await repository.AddAsync(mappedTodo, cancellationToken);

        return Unit.Value;
    }

    public async Task<Unit> DeleteTodoAsync(Guid id, CancellationToken cancellationToken)
    {
        var existingTodo = await repository.GetByIdAsync(id, userClaims.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("Task not found.");

        await repository.RemoveAsync(existingTodo, cancellationToken);

        return Unit.Value;
    }

    public async Task<ToDoDto> GetTodoByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var todo = await repository.GetByIdAsync(id, userClaims.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("Task not found.");

        return mapper.Map<ToDoDto>(todo);
    } 

    public async Task<Unit> UpdateTodoAsync(Guid id, UpdateToDoDto updatedTodo, CancellationToken cancellationToken)
    {
        var existingTodo = await repository.GetByIdAsync(id, userClaims.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("Task not found.");

        existingTodo.Update(updatedTodo.Title, updatedTodo.Description, updatedTodo.IsCompleted);

        await repository.UpdateAsync(existingTodo, cancellationToken);

        return Unit.Value;
    }

    public async Task<List<ToDoDto>> GetAllTodosAsync(CancellationToken cancellationToken)
        => mapper.Map<List<ToDoDto>>(await repository.GetAllByUserIdAsync(userClaims.UserId, cancellationToken));

    public async Task<Unit> DeleteAllTodosAsync(CancellationToken cancellationToken)
    {
        var todos = await repository.GetAllByUserIdAsync(userClaims.UserId, cancellationToken);

        if (todos.Count == 0) throw new KeyNotFoundException("You have no task to remove.");

        await repository.RemoveAllAsync(todos, cancellationToken);

        return Unit.Value;
    }
    #endregion
}