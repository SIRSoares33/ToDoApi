using AutoMapper;
using MediatR;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Queries.Users;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.Users;

public class GetUsersHandler(IUserService service, IMapper mapper) : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        => mapper.Map<List<UserDto>>(await service.GetAllUsersAsync(cancellationToken));
}