using AutoMapper;
using MediatR;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Auth.Queries;
using ToDo.Domain.Interfaces.Repository;

namespace ToDo.Application.Features.Auth.Handlers;

public class GetUsersHandler(IUserRepository repository, IMapper mapper) : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        => mapper.Map<List<UserDto>>(await repository.GetUsersAsync(cancellationToken));
}