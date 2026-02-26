using MediatR;

namespace ToDo.Application.Features.Commands.ToDo;

public record RemoveToDoCommand(Guid? Id) : IRequest<Unit>;
