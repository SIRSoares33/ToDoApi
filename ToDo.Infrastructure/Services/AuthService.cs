using MediatR;
using Microsoft.AspNetCore.Identity;
using Psalms.Auth.Jwt;
using System.Security.Claims;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Responses.Auth;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;
using ToDo.Domain.Interfaces.Repository;
using ToDo.Domain.ValueObjects;
using ToDo.Domain.ValueObjects.Users;

namespace ToDo.Infrastructure.Services;

public class AuthService(
    IUserRepository repository,
    PsalmsJwtTokenService jwtService,
    IPasswordHasher<IAuthService> hasher) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginDto dto, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByEmailAsync(dto.Email, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (hasher.VerifyHashedPassword(this, user.HashPassword.Value, dto.Password) == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Invalid credentials.");

        return new LoginResponse(user.Id, await CreateTokenAsync(user));
    }

    public async Task<Unit> RegisterAsync(User user, Role role, CancellationToken cancellationToken)
    {
        if (await repository.IsEmailRegisteredAsync(user.Email.Value, cancellationToken))
            throw new InvalidOperationException("Email is already registered.");

        user.ChangePassword(new Password(hasher.HashPassword(this, user.HashPassword.Value)));
        user.ChangeRole(role);

        await repository.AddUserAsync(user, cancellationToken);

        return Unit.Value;
    }

    private async Task<string> CreateTokenAsync(User user)
    {
        return await jwtService.GenerateAccessTokenAsync(
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email.Value),
            new Claim(ClaimTypes.Name, user.Name.Value),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        ]);
    }
}