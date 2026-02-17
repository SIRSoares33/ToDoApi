using MediatR;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Auth.Commands;

public record UpdateUserCommand(Guid id, UpdateUserDto dto) : IRequest<Unit>;