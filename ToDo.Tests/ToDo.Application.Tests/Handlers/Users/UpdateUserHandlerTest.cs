using MediatR;
using Moq;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Features.Handlers.Users;
using ToDo.Application.Interfaces;
using ToDo.Domain.Enums;

namespace ToDo.Application.Tests.Handlers.Users;

public class UpdateUserHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_User_When_Role_Is_Null()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        var dto = new UpdateUserDto
        {
            Name = "Updated Name",
            Email = "updated@email.com",
            Role = null
        };

        var serviceMock = new Mock<IUserService>();
        serviceMock
            .Setup(s => s.UpdateUserAsync(userId, dto, cancellationToken))
            .ReturnsAsync(Unit.Value);

        var handler = new UpdateUserHandler(serviceMock.Object);
        var command = new UpdateUserCommand(userId, dto);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        Assert.Equal(Unit.Value, result);

        serviceMock.Verify(
            s => s.UpdateUserAsync(userId, dto, cancellationToken),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_Should_Throw_UnauthorizedAccessException_When_Role_Is_Provided()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        var dto = new UpdateUserDto
        {
            Name = "Updated Name",
            Email = "updated@email.com",
            Role = Role.Admin
        };

        var serviceMock = new Mock<IUserService>();
        var handler = new UpdateUserHandler(serviceMock.Object);
        var command = new UpdateUserCommand(userId, dto);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => handler.Handle(command, cancellationToken)
        );

        Assert.Equal("You cannot change the user's role.", exception.Message);

        serviceMock.Verify(
            s => s.UpdateUserAsync(It.IsAny<Guid>(), It.IsAny<UpdateUserDto>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}