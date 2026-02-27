using MediatR;
using Moq;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Task;
using ToDo.Application.Features.Commands.ToDo;
using ToDo.Application.Features.Handlers.ToDo;
using ToDo.Application.Features.Queries.Task;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Tests.Handlers.ToDo;

public class ToDoHandlersTests
{
    private readonly Mock<ITodoService> _serviceMock;

    public ToDoHandlersTests() => _serviceMock = new Mock<ITodoService>();

    [Fact]
    public async Task AddToDoHandler_Should_Call_CreateTodoAsync()
    {
        var dto = new AddToDoDto { Title = "Task" };
        var command = new AddToDoCommand(dto);

        _serviceMock
            .Setup(s => s.CreateTodoAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var handler = new AddToDoHandler(_serviceMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        _serviceMock.Verify(
            s => s.CreateTodoAsync(dto, It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task DeleteAllTodosHandler_Should_Call_DeleteAllTodosAsync()
    {
        var command = new DeleteAllToDosCommand();

        _serviceMock
            .Setup(s => s.DeleteAllTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var handler = new DeleteAllTodosHandler(_serviceMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        _serviceMock.Verify(
            s => s.DeleteAllTodosAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task DeleteToDoHandler_Should_Call_DeleteTodoAsync()
    {
        var id = Guid.NewGuid();
        var command = new DeleteToDoCommand(id);

        _serviceMock
            .Setup(s => s.DeleteTodoAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var handler = new DeleteToDoHandler(_serviceMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        _serviceMock.Verify(
            s => s.DeleteTodoAsync(id, It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task GetTodoByIdHandler_Should_Return_Todo()
    {
        var id = Guid.NewGuid();
        var expected = new ToDoDto { Id = id, Title = "Task" };
        var query = new GetToDoByIdQuery(id);

        _serviceMock
            .Setup(s => s.GetTodoByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new GetTodoByIdHandler(_serviceMock.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        _serviceMock.Verify(
            s => s.GetTodoByIdAsync(id, It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GetToDosHandler_Should_Return_List()
    {
        var expected = new List<ToDoDto>
        {
            new() { Title = "Task 1" },
            new() { Title = "Task 2" }
        };

        var query = new GetToDosQuery();

        _serviceMock
            .Setup(s => s.GetAllTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new GetToDosHandler(_serviceMock.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        _serviceMock.Verify(
            s => s.GetAllTodosAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task UpdateToDoHandler_Should_Call_UpdateTodoAsync()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateToDoDto { Title = "Updated" };
        var command = new UpdateToDoCommand(id, dto);

        _serviceMock
            .Setup(s => s.UpdateTodoAsync(id, dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var handler = new UpdateToDoHandler(_serviceMock.Object);

        var result = await handler.Handle(command, CancellationToken.None);

        _serviceMock.Verify(
            s => s.UpdateTodoAsync(id, dto, It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.Equal(Unit.Value, result);
    }
}