using MediatR;
using ToDo.Application.DTOs;
using ToDo.Domain.Enums;

namespace ToDo.Application.Features.Commands.Users;

public record AddUserCommand(RegisterDto Dto, Role Role) : IRequest<Unit>;