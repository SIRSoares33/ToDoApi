using MediatR;
using Moq;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Features.Handlers.Users;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Tests.Handlers.Users;

public class UpdateUserByAdminHandlerTests
{
    [Fact]
    public async Task Handle_Should_Call_UpdateUserAsync_And_Return_Unit()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        var dto = new UpdateUserDto
        {
            Name = "Updated Name",
            Email = "updated@email.com"
        };

        var serviceMock = new Mock<IUserService>();
        serviceMock
            .Setup(s => s.UpdateUserAsync(userId, dto, cancellationToken))
            .ReturnsAsync(Unit.Value);

        var handler = new UpdateUserByAdminHandler(serviceMock.Object);
        var command = new UpdateUserByAdminCommand(userId, dto);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        Assert.Equal(Unit.Value, result);

        serviceMock.Verify(
            s => s.UpdateUserAsync(userId, dto, cancellationToken),
            Times.Once
        );
    }
}