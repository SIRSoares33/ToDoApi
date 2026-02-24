using MediatR;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.Users;

public class UpdateUserByAdminHandler(IUserService service) : IRequestHandler<UpdateUserByAdminCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserByAdminCommand request, CancellationToken cancellationToken)
        => await service.UpdateUserAsync(request.id, request.dto, cancellationToken);
}