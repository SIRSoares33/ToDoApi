using MediatR;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Commands.Users;

public record UpdateUserCommand(Guid id, UpdateUserDto dto) : IRequest<Unit>;