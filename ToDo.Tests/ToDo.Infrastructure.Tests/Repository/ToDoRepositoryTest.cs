using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces.Repository;
using ToDo.Domain.ValueObjects.Task;
using ToDo.Infrastructure.Context;
using ToDo.Infrastructure.Repositories;

namespace ToDo.Infrastructure.Tests.Repository;

public class TodoRepositoryTests
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _context;
    private readonly ITodoRepository _repository;

    public TodoRepositoryTests()
    {
        // Banco SQLite em memória (real)
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        _repository = new TodoRepository(_context);
    }

    [Fact]
    public async Task AddAsync_Should_Persist_Todo()
    {
        var todo = new Todo(
            new Title("Test"),
            new Description("Desc"),
            false,
            Guid.NewGuid());

        await _repository.AddAsync(todo, CancellationToken.None);

        Assert.Single(_context.Todos);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Todo_For_User()
    {
        var userId = Guid.NewGuid();

        var todo = new Todo(
            new Title("Title"),
            new Description("Desc"),
            false,
            userId);

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(todo.Id, userId, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(todo.Id, result!.Id);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_For_Wrong_User()
    {
        var todo = new Todo(
            new Title("Title"),
            new Description("Desc"),
            false,
            Guid.NewGuid());

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(todo.Id, Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllByUserIdAsync_Should_Return_Only_User_Todos()
    {
        var userId = Guid.NewGuid();

        _context.Todos.AddRange(
            new Todo(new Title("1"), new Description(), false, userId),
            new Todo(new Title("2"), new Description(), false, userId),
            new Todo(new Title("3"), new Description(), false, Guid.NewGuid())
        );

        await _context.SaveChangesAsync();

        var result = await _repository.GetAllByUserIdAsync(userId, CancellationToken.None);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Todo()
    {
        var todo = new Todo(
            new Title("Old"),
            new Description("Desc"),
            false,
            Guid.NewGuid());

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        todo.Update("New", null, null);

        await _repository.UpdateAsync(todo, CancellationToken.None);

        var updated = await _context.Todos.FirstAsync();
        Assert.Equal("New", updated.Title.Value);
    }

    [Fact]
    public async Task RemoveAsync_Should_Delete_Todo()
    {
        var todo = new Todo(
            new Title("Title"),
            new Description(),
            false,
            Guid.NewGuid());

        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();

        await _repository.RemoveAsync(todo, CancellationToken.None);

        Assert.Empty(_context.Todos);
    }

    [Fact]
    public async Task RemoveAllAsync_Should_Delete_All()
    {
        _context.Todos.AddRange(
            new Todo(new Title("1"), new Description(), false, Guid.NewGuid()),
            new Todo(new Title("2"), new Description(), false, Guid.NewGuid())
        );

        await _context.SaveChangesAsync();

        await _repository.RemoveAllAsync(_context.Todos.ToList(), CancellationToken.None);

        Assert.Empty(_context.Todos);
    }
}