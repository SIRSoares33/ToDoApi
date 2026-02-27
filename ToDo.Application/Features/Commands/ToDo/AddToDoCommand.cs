using MediatR;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Commands.Task;

public record AddToDoCommand(AddToDoDto Dto) : IRequest<Unit>;