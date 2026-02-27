using MediatR;
using ToDo.Application.Features.Commands.Task;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.ToDo;

public class AddToDoHandler(ITodoService service) : IRequestHandler<AddToDoCommand, Unit>
{
    public async Task<Unit> Handle(AddToDoCommand request, CancellationToken cancellationToken)
        => await service.CreateTodoAsync(request.Dto, cancellationToken);
}