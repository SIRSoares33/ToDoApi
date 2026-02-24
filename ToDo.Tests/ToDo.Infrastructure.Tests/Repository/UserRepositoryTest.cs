using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;
using ToDo.Domain.ValueObjects;
using ToDo.Infrastructure.Context;
using ToDo.Infrastructure.Repositories;

namespace ToDo.Infrastructure.Tests.Repository;

public class UserRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        // Banco SQLite em memória (real)
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        _repository = new UserRepository(_context);
    }

    // --------------------
    // ADD
    // --------------------

    [Fact]
    public async Task AddUserAsync_Should_Persist_User()
    {
        // Arrange
        var user = CreateUser("add@email.com");

        // Act
        await _repository.AddUserAsync(user, CancellationToken.None);

        // Assert
        var savedUser = await _context.Users.FirstOrDefaultAsync();
        savedUser.Should().NotBeNull();
        savedUser!.Email.Value.Should().Be("add@email.com");
    }

    // --------------------
    // GET BY EMAIL
    // --------------------

    [Fact]
    public async Task GetUserByEmailAsync_Should_Return_User_When_Exists()
    {
        // Arrange
        var user = CreateUser("email@email.com");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetUserByEmailAsync(
            "email@email.com",
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
    }

    // --------------------
    // GET BY ID
    // --------------------

    [Fact]
    public async Task GetUserByIdAsync_Should_Return_User_When_Exists()
    {
        // Arrange
        var user = CreateUser("id@email.com");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetUserByIdAsync(
            user.Id,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Value.Should().Be("id@email.com");
    }

    // --------------------
    // UPDATE
    // --------------------

    [Fact]
    public async Task UpdateUserAsync_Should_Update_User_Data()
    {
        // Arrange
        var user = CreateUser("old@email.com");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        user.Update(
            name: "Updated Name",
            email: "new@email.com",
            hashedPassword: null,
            role: Role.Admin);

        // Act
        await _repository.UpdateUserAsync(user, CancellationToken.None);

        // Assert
        var updatedUser = await _context.Users.FirstAsync();
        updatedUser.Email.Value.Should().Be("new@email.com");
        updatedUser.Role.Should().Be(Role.Admin);
    }

    // --------------------
    // DELETE
    // --------------------

    [Fact]
    public async Task DeleteUserAsync_Should_Remove_User()
    {
        // Arrange
        var user = CreateUser("delete@email.com");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteUserAsync(user, CancellationToken.None);

        // Assert
        var count = await _context.Users.CountAsync();
        count.Should().Be(0);
    }

    // --------------------
    // EMAIL EXISTS
    // --------------------

    [Fact]
    public async Task IsEmailRegisteredAsync_Should_Return_True_When_Email_Exists()
    {
        // Arrange
        var user = CreateUser("exists@email.com");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _repository.IsEmailRegisteredAsync(
            "exists@email.com",
            CancellationToken.None);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task IsEmailRegisteredAsync_Should_Return_False_When_Email_Does_Not_Exist()
    {
        // Act
        var exists = await _repository.IsEmailRegisteredAsync(
            "notfound@email.com",
            CancellationToken.None);

        // Assert
        exists.Should().BeFalse();
    }

    // --------------------
    // GET ALL
    // --------------------

    [Fact]
    public async Task GetUsersAsync_Should_Return_All_Users_As_NoTracking()
    {
        // Arrange
        _context.Users.AddRange(
            CreateUser("1@email.com"),
            CreateUser("2@email.com"));
        await _context.SaveChangesAsync();

        // Act
        var users = await _repository.GetUsersAsync(CancellationToken.None);

        // Assert
        users.Should().HaveCount(2);
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
            Role.User);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}