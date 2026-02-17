using MediatR;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.Users;

public class DeleteUserHandler(IUserService service) : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        => await service.DeleteUserAsync(request.UserId, cancellationToken);
}