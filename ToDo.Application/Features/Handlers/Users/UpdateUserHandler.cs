using AutoMapper;
using MediatR;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;

namespace ToDo.Application.Features.Handlers.Users;

public class UpdateUserHandler(IUserService service, IMapper mapper) : IRequestHandler<UpdateUserCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        => await service.UpdateUserAsync(request.id, mapper.Map<User>(request.dto), cancellationToken);
}