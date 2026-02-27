using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Features.Queries.Users;
using ToDo.Application.Interfaces;
using Todo.Api.Controllers;

namespace Todo.Api.Tests.Controllers;

public class UsersControllerTests
{
    #region Attributes
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IUserClaims> _userClaimsMock;
    private readonly UsersController _controller;
    #endregion

    #region Constructor
    public UsersControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _userClaimsMock = new Mock<IUserClaims>();

        _controller = new UsersController(
            _mediatorMock.Object,
            _userClaimsMock.Object);
    }
    #endregion

    #region Tests

    // --------------------
    // GET /users (Admin)
    // --------------------
    [Fact]
    public async Task GetUsers_Should_Return_Ok()
    {
        // Arrange
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var result = await _controller.GetUsers(CancellationToken.None);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        _mediatorMock.Verify(
            x => x.Send(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // --------------------
    // PUT /users (Own Account)
    // --------------------
    [Fact]
    public async Task UpdateOwnAccount_Should_Return_NoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userClaimsMock.Setup(x => x.UserId).Returns(userId);

        var dto = new UpdateUserDto
        {
            Name = "User Update",
            Email = "user@email.com"
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.UpdateOwnAccount(dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<UpdateUserCommand>(c =>
                    c.id == userId &&
                    c.dto == dto),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // --------------------
    // PUT /users/{id} (Admin)
    // --------------------
    [Fact]
    public async Task UpdateUserByAdmin_Should_Return_NoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var dto = new UpdateUserDto
        {
            Name = "Admin Update",
            Email = "admin@email.com"
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<UpdateUserByAdminCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.UpdateUser(userId, dto, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<UpdateUserByAdminCommand>(c =>
                    c.id == userId &&
                    c.dto == dto),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // --------------------
    // DELETE /users (Own Account)
    // --------------------
    [Fact]
    public async Task DeleteOwnAccount_Should_Return_NoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userClaimsMock.Setup(x => x.UserId).Returns(userId);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.DeleteOwnAccount(CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<DeleteUserCommand>(c => c.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // --------------------
    // DELETE /users/{id} (Admin)
    // --------------------
    [Fact]
    public async Task DeleteUserByAdmin_Should_Return_NoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.DeleteUser(userId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<DeleteUserCommand>(c => c.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}