using AutoMapper;
using MediatR;
using Moq;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Features.Handlers.Users;
using ToDo.Application.Interfaces;  
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;
using ToDo.Domain.ValueObjects;

namespace ToDo.Application.Tests.Handlers.Users;

public class AddUserHandlerTests
{
    [Fact]
    public async Task Handle_Should_Map_Dto_And_Call_RegisterAsync()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Name = "Gustavo",
            Email = "gustavo@example.com",
            Password = "Abc123!@"
        };
        var role = Role.User;
        var command = new AddUserCommand(dto, role);

        var user = new User(new Name(dto.Name), new Email(dto.Email), null, role);

        // Mock do IMapper
        var mapperMock = new Mock<IMapper>();
        mapperMock
            .Setup(m => m.Map<User>(dto))
            .Returns(user);

        // Mock do IAuthService
        var authServiceMock = new Mock<IAuthService>();
        authServiceMock
            .Setup(s => s.RegisterAsync(user, role, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var handler = new AddUserHandler(authServiceMock.Object, mapperMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        mapperMock.Verify(m => m.Map<User>(dto), Times.Once);
        authServiceMock.Verify(s => s.RegisterAsync(user, role, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Propagate_Exception_From_Service()
    {
        // Arrange
        var dto = new RegisterDto { Name = "Gustavo", Email = "gustavo@example.com", Password = "Abc123!@" };
        var role = Role.User;
        var command = new AddUserCommand(dto, role);

        var user = new User(new Name(dto.Name), new Email(dto.Email), null, role);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<User>(dto)).Returns(user);

        var authServiceMock = new Mock<IAuthService>();
        authServiceMock
            .Setup(s => s.RegisterAsync(user, role, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("User already exists"));

        var handler = new AddUserHandler(authServiceMock.Object, mapperMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}