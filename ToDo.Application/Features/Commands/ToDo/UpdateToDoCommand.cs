using MediatR;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Commands.Task;

public record UpdateToDoCommand(Guid Id, UpdateToDoDto Dto) : IRequest<Unit>;