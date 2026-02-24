using MediatR;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Queries.Users;

public record GetUsersQuery : IRequest<List<UserDto>>;