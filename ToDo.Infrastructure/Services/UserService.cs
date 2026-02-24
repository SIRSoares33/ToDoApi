using MediatR;
using Microsoft.AspNetCore.Identity;
using ToDo.Application.DTOs;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces.Repository;

namespace ToDo.Infrastructure.Services;

public class UserService(IUserRepository userRepository, IPasswordHasher<IAuthService> hasher) : IUserService
{
    #region IUserService Implementation
    public async Task<Unit> DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(userId, cancellationToken)
            ?? throw new UnauthorizedAccessException("Id not found.");

        await userRepository.DeleteUserAsync(user, cancellationToken);

        return Unit.Value;
    }

    public async Task<Unit> UpdateUserAsync(Guid id, UpdateUserDto dto, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(id, cancellationToken)
            ?? throw new UnauthorizedAccessException("User id not found.");

        dto.Password = dto.Password is null ? null : hasher.HashPassword(null, dto.Password!);

        if (await IsEmailRegisteredAsync(dto.Email, cancellationToken))
            throw new UnauthorizedAccessException("Email is already registered.");

        user.Update(dto.Name, dto.Email, dto.Password, dto.Role);

        await userRepository.UpdateUserAsync(user, cancellationToken);

        return Unit.Value;
    }

    public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        => await userRepository.GetUsersAsync(cancellationToken);
    #endregion

    #region Private Helper Methods
    private async Task<bool> IsEmailRegisteredAsync(string? emailInput, CancellationToken cancellationToken)
        => emailInput is not null && await userRepository.IsEmailRegisteredAsync(emailInput, cancellationToken);
    #endregion
}