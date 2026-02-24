using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Todo.Api.Controllers;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Features.Queries.Users;

namespace ToDo.Api.Tests.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UsersController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetUsers_Should_Return_Ok()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var result = await _controller.GetUsers(CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        _mediatorMock.Verify(m =>
            m.Send(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateUser_Should_Return_NoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateUserDto() {Name="Name", Email="email@test.com", Password=null, Role=null};

        // Act
        var result = await _controller.UpdateUser(id, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mediatorMock.Verify(m =>
            m.Send(It.Is<UpdateUserByAdminCommand>(c =>
                c.dto == dto),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateOwnAccount_Should_Return_NoContent_When_User_Is_Authenticated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUser(userId);

        var dto = new UpdateUserDto() { Name = "Name", Email = "email@test.com", Password = null, Role = null };

        // Act
        var result = await _controller.UpdateOwnAccount(dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mediatorMock.Verify(m =>
            m.Send(It.Is<UpdateUserCommand>(c =>
                c.id == userId &&
                c.dto == dto),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateOwnAccount_Should_Return_Unauthorized_When_Claim_Is_Invalid()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act
        var result = await _controller.UpdateOwnAccount(
            new UpdateUserDto() { Name = "Name", Email = "email@test.com", Password = null, Role = null },
        CancellationToken.None);

        // Assert
        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task DeleteUser_Should_Return_NoContent()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var result = await _controller.DeleteUser(id, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mediatorMock.Verify(m =>
            m.Send(It.Is<DeleteUserCommand>(c => c.UserId == id),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteOwnAccount_Should_Return_NoContent_When_Authenticated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUser(userId);

        // Act
        var result = await _controller.DeleteOwnAccount(CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mediatorMock.Verify(m =>
            m.Send(It.Is<DeleteUserCommand>(c => c.UserId == userId),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private void SetUser(Guid userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };
    }
}