using MediatR;
using Moq;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Features.Handlers.Users;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Tests.Handlers.Users;

public class DeleteUserHandlerTests
{
    [Fact]
    public async Task Handle_Should_Call_DeleteUserAsync_And_Return_Unit()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(s => s.DeleteUserAsync(userId, cancellationToken))
            .ReturnsAsync(Unit.Value);

        var handler = new DeleteUserHandler(userServiceMock.Object);
        var command = new DeleteUserCommand(userId);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        Assert.Equal(Unit.Value, result);

        userServiceMock.Verify(
            s => s.DeleteUserAsync(userId, cancellationToken),
            Times.Once
        );
    }
}