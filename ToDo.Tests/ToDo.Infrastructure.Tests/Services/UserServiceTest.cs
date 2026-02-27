using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using ToDo.Application.DTOs;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;
using ToDo.Domain.Interfaces.Repository;
using ToDo.Domain.ValueObjects;
using ToDo.Domain.ValueObjects.Users;
using ToDo.Infrastructure.Services;

namespace ToDo.Infrastructure.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher<IAuthService>> _passwordHasherMock;

    private readonly UserService _sut;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<IAuthService>>();

        _sut = new UserService(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object
        );
    }

    // --------------------
    // DELETE
    // --------------------

    [Fact]
    public async Task DeleteUserAsync_Should_Delete_User_When_Id_Exists()
    {
        // Arrange
        var user = CreateUser();

        _userRepositoryMock
            .Setup(x => x.GetUserByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _sut.DeleteUserAsync(user.Id, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        _userRepositoryMock.Verify(
            x => x.DeleteUserAsync(user, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_Should_Throw_When_User_Does_Not_Exist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(x => x.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () =>
            await _sut.DeleteUserAsync(userId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Id not found.");

        _userRepositoryMock.Verify(
            x => x.DeleteUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // --------------------
    // UPDATE
    // --------------------

    [Fact]
    public async Task UpdateUserAsync_Should_Update_User_When_Data_Is_Valid()
    {
        // Arrange
        var user = CreateUser();

        var dto = new UpdateUserDto
        {
            Name = "Updated Name",
            Email = "updated@email.com",
            Password = "Password123!",
            Role = Role.Admin
        };

        _userRepositoryMock
            .Setup(x => x.GetUserByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.IsEmailRegisteredAsync(dto.Email!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _passwordHasherMock
            .Setup(x => x.HashPassword(null, dto.Password!))
            .Returns("Password123!");

        // Act
        var result = await _sut.UpdateUserAsync(user.Id, dto, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        _userRepositoryMock.Verify(
            x => x.UpdateUserAsync(user, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_Should_Throw_When_User_Does_Not_Exist()
    {
        // Arrange
        var dto = new UpdateUserDto
        {
            Name = "Any",
            Email = "any@email.com"
        };

        _userRepositoryMock
            .Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () =>
            await _sut.UpdateUserAsync(Guid.NewGuid(), dto, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("User id not found.");
    }

    [Fact]
    public async Task UpdateUserAsync_Should_Throw_When_Email_Is_Already_Registered()
    {
        // Arrange
        var user = CreateUser();

        var dto = new UpdateUserDto
        {
            Email = "existing@email.com"
        };

        _userRepositoryMock
            .Setup(x => x.GetUserByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.IsEmailRegisteredAsync(dto.Email!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () =>
            await _sut.UpdateUserAsync(user.Id, dto, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Email is already registered.");

        _userRepositoryMock.Verify(
            x => x.UpdateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateUserAsync_Should_Not_Hash_Password_When_Password_Is_Null()
    {
        // Arrange
        var user = CreateUser();

        var dto = new UpdateUserDto
        {
            Name = "Updated",
            Email = "updated@email.com",
            Password = null
        };

        _userRepositoryMock
            .Setup(x => x.GetUserByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.IsEmailRegisteredAsync(dto.Email!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        await _sut.UpdateUserAsync(user.Id, dto, CancellationToken.None);

        // Assert
        _passwordHasherMock.Verify(
            x => x.HashPassword(It.IsAny<IAuthService>(), It.IsAny<string>()),
            Times.Never);
    }

    // --------------------
    // GET ALL
    // --------------------

    [Fact]
    public async Task GetAllUsersAsync_Should_Return_List_Of_Users()
    {
        // Arrange
        var users = new List<User>
        {
            CreateUser(),
            CreateUser()
        };

        _userRepositoryMock
            .Setup(x => x.GetUsersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _sut.GetAllUsersAsync(CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    // --------------------
    // HELPERS
    // --------------------

    private static User CreateUser()
    {
        return new User(
            new Name("Gustavo"),
            new Email("email@email.com"),
            new Password("Password123!"),
            Role.User
        );
    }
}