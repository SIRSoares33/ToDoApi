using MediatR;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.Users;

/// <summary>
/// handler for the UpdateUserCommand, responsible for processing the command to update a user's information.
/// </summary>
/// <param name="service"></param>
public class UpdateUserHandler(IUserService service) : IRequestHandler<UpdateUserCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.dto.Role.HasValue) throw new UnauthorizedAccessException("You cannot change the user's role.");

        return await service.UpdateUserAsync(request.id, request.dto, cancellationToken);
    }
}