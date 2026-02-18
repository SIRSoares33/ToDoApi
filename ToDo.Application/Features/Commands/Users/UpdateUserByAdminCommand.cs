using MediatR;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Commands.Users;

public record UpdateUserByAdminCommand(Guid id, UpdateUserDto dto) : IRequest<Unit>;