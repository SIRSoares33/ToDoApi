using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Auth;
using ToDo.Application.Features.Commands.Users;
using ToDo.Domain.Enums;
using Todo.Api.Controllers;
using ToDo.Application.Features.Responses.Auth;

namespace Todo.Api.Tests.Controllers;

public class AuthControllerTests
{
    #region Attributes
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;
    #endregion

    #region Constructor
    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController(_mediatorMock.Object);
    }
    #endregion

    #region Tests
    // --------------------
    //       LOGIN        |
    // --------------------
    [Fact]
    public async Task Login_Should_Return_Ok_With_Response()
    {
        // Arrange
        var dto = new LoginDto() { Email= "email@email.com", Password="123456" };

        var expectedResponse = new LoginResponse(Guid.NewGuid(), "fake-jwt-token");

        _mediatorMock.Setup (x => x.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Login(dto, CancellationToken.None);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(expectedResponse);

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<LoginCommand>(c => c.LoginDto == dto),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // --------------------
    // REGISTER USER
    // --------------------

    [Fact]
    public async Task Register_Should_Return_Created()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Name = "Gustavo",
            Email = "email@email.com",
            Password = "123456"
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<AddUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.Register(dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedResult>();

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<AddUserCommand>(c =>
                    c.Role == Role.User &&
                    c.Dto == dto),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // --------------------
    // REGISTER ADMIN
    // --------------------

    [Fact]
    public async Task RegisterAdmin_Should_Send_Admin_Command_And_Return_Created()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Name = "Admin",
            Email = "admin@email.com",
            Password = "admin123"
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<AddUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.RegisterAdmin(dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedResult>();

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<AddUserCommand>(c =>
                    c.Role == Role.Admin &&
                    c.Dto == dto),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    #endregion
}