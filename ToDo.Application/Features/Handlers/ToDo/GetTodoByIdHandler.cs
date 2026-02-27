using MediatR;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Queries.Task;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.ToDo;

public class GetTodoByIdHandler(ITodoService service) : IRequestHandler<GetToDoByIdQuery, ToDoDto>
{
    public async Task<ToDoDto> Handle(GetToDoByIdQuery request, CancellationToken cancellationToken)
        => await service.GetTodoByIdAsync(request.Id, cancellationToken);
}
