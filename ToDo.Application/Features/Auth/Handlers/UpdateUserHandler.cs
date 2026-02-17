using MediatR;
using Microsoft.AspNetCore.Identity;
using ToDo.Application.Features.Auth.Commands;
using ToDo.Application.Interfaces;
using ToDo.Domain.Interfaces.Repository;

namespace ToDo.Application.Features.Auth.Handlers;

public class UpdateUserHandler(IUserRepository userRepository, IPasswordHasher<IAuthService> hasher) : IRequestHandler<UpdateUserCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.id, cancellationToken)
            ?? throw new UnauthorizedAccessException("User id not found.");

        request.dto.Password = request.dto.Password is not null ? hasher.HashPassword(null, request.dto.Password) : null;

        if (!string.IsNullOrEmpty(request.dto.Email) && request.dto.Email != user.Email.Value &&
            await userRepository.IsEmailRegisteredAsync(request.dto.Email, cancellationToken))
            throw new UnauthorizedAccessException("Email is already registered.");

        user.Update(request.dto.Name, request.dto.Email, request.dto.Password, request.dto.Role);

        await userRepository.UpdateUserAsync(user, cancellationToken);

        return Unit.Value;
    }
}