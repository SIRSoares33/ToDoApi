using MediatR;
using ToDo.Application.Features.Commands.ToDo;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.ToDo;

public class DeleteToDoHandler(ITodoService service) : IRequestHandler<RemoveToDoCommand, Unit>
{
    public async Task<Unit> Handle(RemoveToDoCommand request, CancellationToken cancellationToken)
       => request.Id is null ? 
        await service.DeleteAllTodosAsync(cancellationToken)
        :
        await service.DeleteTodoAsync(request.Id.Value, cancellationToken);
}