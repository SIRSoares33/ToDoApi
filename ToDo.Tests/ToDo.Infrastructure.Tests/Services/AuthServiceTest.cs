using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Psalms.Auth.Jwt;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Responses.Auth;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;
using ToDo.Domain.Interfaces.Repository;
using ToDo.Domain.ValueObjects;
using ToDo.Infrastructure.Services;

namespace ToDo.Infrastructure.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher<IAuthService>> _passwordHasherMock;
    private readonly IConfigurationRoot _config;

    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        _passwordHasherMock = new Mock<IPasswordHasher<IAuthService>>();

        _config = new ConfigurationBuilder().
            AddInMemoryCollection(new Dictionary<string, string?>{
                ["Jwt:Key"] = "test-secret-key-1234583789327r88237r3r9832r6789"}).Build();

        _sut = new AuthService(
            _userRepositoryMock.Object,
            new PsalmsJwtTokenService(_config),
            _passwordHasherMock.Object
        );
    }

    // --------------------
    // LOGIN
    // --------------------

    [Fact]
    public async Task LoginAsync_Should_Return_LoginResponse_When_Credentials_Are_Valid()
    {
        // Arrange
        var dto = new LoginDto() { Email = "email@email.com", Password = "123456" };

        var user = CreateUser(dto.Email);

        _userRepositoryMock
            .Setup(x => x.GetUserByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(x => x.VerifyHashedPassword(
                It.IsAny<IAuthService>(),
                user.HashPassword.Value,
                dto.Password))
            .Returns(PasswordVerificationResult.Success);


        // Act
        var result = await _sut.LoginAsync(dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<LoginResponse>();
        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task LoginAsync_Should_Throw_When_User_Does_Not_Exist()
    {
        // Arrange
        var dto = new LoginDto() { Email = "email@email.com", Password = "123456" };

        _userRepositoryMock
            .Setup(x => x.GetUserByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () => await _sut.LoginAsync(dto, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid credentials.");
    }

    [Fact]
    public async Task LoginAsync_Should_Throw_When_Password_Is_Invalid()
    {
        // Arrange
        var dto = new LoginDto() { Email = "email@email.com", Password = "WrongPassword123!" };
        var user = CreateUser(dto.Email);

        _userRepositoryMock
            .Setup(x => x.GetUserByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(x => x.VerifyHashedPassword(
                It.IsAny<IAuthService>(),
                user.HashPassword.Value,
                dto.Password))
            .Returns(PasswordVerificationResult.Failed);

        // Act
        var act = async () => await _sut.LoginAsync(dto, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid credentials.");
    }

    // --------------------
    // REGISTER
    // --------------------

    [Fact]
    public async Task RegisterAsync_Should_Create_User_When_Email_Is_Not_Registered()
    {
        // Arrange
        var user = CreateUser("email@email.com");

        _userRepositoryMock
            .Setup(x => x.IsEmailRegisteredAsync(user.Email.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _passwordHasherMock
            .Setup(x => x.HashPassword(
                It.IsAny<IAuthService>(),
                user.HashPassword.Value))
            .Returns("Password123!");

        // Act
        var result = await _sut.RegisterAsync(user, Role.User, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        _userRepositoryMock.Verify(
            x => x.AddUserAsync(user, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_Should_Throw_When_Email_Is_Already_Registered()
    {
        // Arrange
        var user = CreateUser("email@email.com");

        _userRepositoryMock
            .Setup(x => x.IsEmailRegisteredAsync(user.Email.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () =>
            await _sut.RegisterAsync(user, Role.User, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Email is already registered.");

        _userRepositoryMock.Verify(
            x => x.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // --------------------
    // HELPERS
    // --------------------

    private static User CreateUser(string email)
    {
        return new User(
            new Name("Gustavo"),
            new Email(email),
            new Password("Password123!"),
            Role.User
        );
    }
}