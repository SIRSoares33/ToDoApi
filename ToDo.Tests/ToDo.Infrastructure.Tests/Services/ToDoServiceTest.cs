using AutoMapper;
using Moq;
using ToDo.Application.DTOs;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Domain.Interfaces.Repository;
using ToDo.Infrastructure.Services;

namespace ToDo.Infrastructure.Tests.Services;

public class TodoServiceTests
{
    private readonly Mock<ITodoRepository> _repository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IUserClaims> _userClaims;
    private readonly TodoService _service;

    private readonly Guid _userId = Guid.NewGuid();

    public TodoServiceTests()
    {
        _repository = new Mock<ITodoRepository>();
        _mapper = new Mock<IMapper>();
        _userClaims = new Mock<IUserClaims>();

        _userClaims.Setup(u => u.UserId).Returns(_userId);

        _service = new TodoService(
            _repository.Object,
            _mapper.Object,
            _userClaims.Object);
    }

    [Fact]
    public async Task CreateTodoAsync_Should_Map_Set_UserId_And_Add()
    {
        // Arrange
        var dto = new AddToDoDto();
        var todo = new Todo();

        _mapper.Setup(m => m.Map<Todo>(dto)).Returns(todo);

        // Act
        await _service.CreateTodoAsync(dto, CancellationToken.None);

        // Assert
        Assert.Equal(_userId, todo.UserId);

        _repository.Verify(
            r => r.AddAsync(todo, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetTodoByIdAsync_Should_Return_Mapped_Dto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var todo = new Todo();
        var dto = new ToDoDto();

        _repository
            .Setup(r => r.GetByIdAsync(id, _userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todo);

        _mapper.Setup(m => m.Map<ToDoDto>(todo)).Returns(dto);

        // Act
        var result = await _service.GetTodoByIdAsync(id, CancellationToken.None);

        // Assert
        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task GetTodoByIdAsync_Should_Throw_When_Not_Found()
    {
        // Arrange
        _repository
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), _userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Todo?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetTodoByIdAsync(Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task UpdateTodoAsync_Should_Update_And_Save()
    {
        // Arrange
        var id = Guid.NewGuid();
        var todo = new Todo(
            new("Title"),
            new("Desc"),
            false,
            _userId);

        var dto = new UpdateToDoDto
        {
            Title = "New title",
            Description = "New desc",
            IsCompleted = true
        };

        _repository
            .Setup(r => r.GetByIdAsync(id, _userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todo);

        // Act
        await _service.UpdateTodoAsync(id, dto, CancellationToken.None);

        // Assert
        Assert.Equal("New title", todo.Title.Value);
        Assert.True(todo.IsCompleted);

        _repository.Verify(
            r => r.UpdateAsync(todo, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteTodoAsync_Should_Remove_When_Exists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var todo = new Todo();

        _repository
            .Setup(r => r.GetByIdAsync(id, _userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todo);

        // Act
        await _service.DeleteTodoAsync(id, CancellationToken.None);

        // Assert
        _repository.Verify(
            r => r.RemoveAsync(todo, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAllTodosAsync_Should_Throw_When_Empty()
    {
        // Arrange
        _repository
            .Setup(r => r.GetAllByUserIdAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteAllTodosAsync(CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAllTodosAsync_Should_Remove_All_When_Exists()
    {
        // Arrange
        var todos = new List<Todo> { new(), new() };

        _repository
            .Setup(r => r.GetAllByUserIdAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(todos);

        // Act
        await _service.DeleteAllTodosAsync(CancellationToken.None);

        // Assert
        _repository.Verify(
            r => r.RemoveAllAsync(todos, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}