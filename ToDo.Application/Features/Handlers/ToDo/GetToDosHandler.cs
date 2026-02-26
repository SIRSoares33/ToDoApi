using MediatR;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Queries.Task;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.ToDo;

public class GetToDosHandler(ITodoService service) : IRequestHandler<GetToDosQuery, List<ToDoDto>>
{
    public async Task<List<ToDoDto>> Handle(GetToDosQuery request, CancellationToken cancellationToken)
        => await service.GetAllTodosAsync(cancellationToken);
}