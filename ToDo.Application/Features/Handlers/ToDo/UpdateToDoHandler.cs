using MediatR;
using ToDo.Application.Features.Commands.Task;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.ToDo;

public class UpdateToDoHandler(ITodoService service) : IRequestHandler<UpdateToDoCommand, Unit>
{
    public async Task<Unit> Handle(UpdateToDoCommand request, CancellationToken cancellationToken)
        => await service.UpdateTodoAsync(request.Id, request.Dto, cancellationToken);
}