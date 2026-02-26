using MediatR;
using ToDo.Application.Features.Commands.ToDo;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.ToDo;

public class DeleteToDoHandler(ITodoService service) : IRequestHandler<DeleteToDoCommand, Unit>
{
    public async Task<Unit> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
       => await service.DeleteTodoAsync(request.Id, cancellationToken);
}