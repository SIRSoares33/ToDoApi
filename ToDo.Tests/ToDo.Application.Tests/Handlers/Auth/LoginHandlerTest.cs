using Moq;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Auth;
using ToDo.Application.Features.Handlers.Auth;
using ToDo.Application.Features.Responses.Auth;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Tests.Handlers.Auth;

public class LoginHandlerTests
{
    [Fact]
    public async Task Handle_Should_Call_LoginAsync_And_Return_Response()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "gustavo@example.com",
            Password = "Abc123!@"
        };

        var expectedResponse = new LoginResponse
        (
           Guid.NewGuid(),
           "mocked"
        );

        var authServiceMock = new Mock<IAuthService>();
        authServiceMock
            .Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var handler = new LoginHandler(authServiceMock.Object);
        var command = new LoginCommand(loginDto);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse.Token, response.Token);
        authServiceMock.Verify(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Propagate_Exception_From_AuthService()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "gustavo@example.com",
            Password = "Abc123!@"
        };

        var authServiceMock = new Mock<IAuthService>();
        authServiceMock
            .Setup(s => s.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Invalid credentials"));

        var handler = new LoginHandler(authServiceMock.Object);
        var command = new LoginCommand(loginDto);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}