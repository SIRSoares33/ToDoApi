using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDo.Api.Controllers;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Task;
using ToDo.Application.Features.Commands.ToDo;
using ToDo.Application.Features.Queries.Task;

namespace ToDo.Api.Tests.Controllers;

public class TasksControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new TasksController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreateTask_Should_Return_Created()
    {
        // Arrange
        var dto = new AddToDoDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AddToDoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.CreateTask(dto, CancellationToken.None);

        // Assert
        Assert.IsType<CreatedResult>(result);
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<AddToDoCommand>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetTasks_Should_Return_Ok_With_List()
    {
        // Arrange
        var tasks = new List<ToDoDto>();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetToDosQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tasks);

        // Act
        var result = await _controller.GetTasks(CancellationToken.None);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(tasks, ok.Value);
    }

    [Fact]
    public async Task GetTask_Should_Return_Ok()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new ToDoDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetToDoByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        var result = await _controller.GetTask(taskId, CancellationToken.None);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(task, ok.Value);
    }

    [Fact]
    public async Task UpdateTask_Should_Return_NoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateToDoDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateToDoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.UpdateTask(id, dto, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteTask_Should_Return_NoContent()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteToDoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.DeleteTask(id, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAllTasks_Should_Return_NoContent()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteAllToDosCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.DeleteAllTasks(CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}