using MediatR;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Queries.Task;

public record GetToDosQuery() : IRequest<List<ToDoDto>>;

public record GetToDoByIdQuery(Guid Id) : IRequest<ToDoDto>;