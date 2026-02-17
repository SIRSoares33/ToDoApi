using MediatR;
using ToDo.Application.DTOs;
using ToDo.Domain.Enums;

namespace ToDo.Application.Features.Auth.Commands;

public record AddUserCommand(RegisterDto Dto, Role Role) : IRequest<Unit>;