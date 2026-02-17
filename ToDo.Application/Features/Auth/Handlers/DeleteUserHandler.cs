using MediatR;
using ToDo.Application.Features.Auth.Commands;
using ToDo.Domain.Interfaces.Repository;

namespace ToDo.Application.Features.Auth.Handlers;

public class DeleteUserHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken)
            ?? throw new UnauthorizedAccessException("Id not found.");

        await userRepository.DeleteUserAsync(user, cancellationToken);

        return Unit.Value;
    }
}
