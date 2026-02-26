using MediatR;

namespace ToDo.Application.Features.Commands.ToDo;

public record DeleteToDoCommand(Guid Id) : IRequest<Unit>;

public record DeleteAllToDosCommand() : IRequest<Unit>;
