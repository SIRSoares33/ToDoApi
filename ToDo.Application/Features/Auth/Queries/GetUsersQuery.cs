using MediatR;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Auth.Queries;

public record GetUsersQuery : IRequest<List<UserDto>>;