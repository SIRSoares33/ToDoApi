using MediatR;
using ToDo.Application.Features.Commands.ToDo;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.ToDo;

public class DeleteAllTodosHandler(ITodoService service) : IRequestHandler<DeleteAllToDosCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAllToDosCommand request, CancellationToken cancellationToken)
        => await service.DeleteAllTodosAsync(cancellationToken);
}