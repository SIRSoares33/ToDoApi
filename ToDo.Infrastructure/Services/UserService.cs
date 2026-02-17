using MediatR;
using Microsoft.AspNetCore.Identity;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces.Repository;
using ToDo.Domain.ValueObjects;

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

    public async Task<Unit> UpdateUserAsync(Guid id, User userInput, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(id, cancellationToken)
            ?? throw new UnauthorizedAccessException("User id not found.");

        if (!string.IsNullOrEmpty(userInput.HashPassword.Value))
            userInput.ChangePassword(await HashPasswordAsync(userInput.HashPassword.Value));

        if (await IsEmailRegisteredAsync(userInput.Email.Value, user.Email.Value, cancellationToken))
            throw new UnauthorizedAccessException("Email is already registered.");

        user.Update(userInput);

        await userRepository.UpdateUserAsync(user, cancellationToken);

        return Unit.Value;
    }

    public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        => await userRepository.GetUsersAsync(cancellationToken);
    #endregion

    #region Private Helper Methods
    private async Task<bool> IsEmailRegisteredAsync(string emailInput, string userEmail, CancellationToken cancellationToken)
        => !string.IsNullOrEmpty(emailInput) && emailInput != userEmail && await userRepository.IsEmailRegisteredAsync(emailInput, cancellationToken);

    private Task<Password> HashPasswordAsync(string password)
        => Task.FromResult(new Password(hasher.HashPassword(null, password)));
    #endregion
}