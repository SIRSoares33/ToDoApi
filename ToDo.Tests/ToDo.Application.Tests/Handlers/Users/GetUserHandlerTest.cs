using AutoMapper;
using Moq;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Handlers.Users;
using ToDo.Application.Features.Queries.Users;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;
using ToDo.Domain.ValueObjects;

namespace ToDo.Application.Tests.Handlers.Users;

public class GetUsersHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Mapped_UserDto_List()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var users = new List<User>
        {
            new (new Name("UserOne"), new Email("email1@email.com"), new Password("Password123@")),
            new (new Name("UserTwo"), new Email("email2@email.com"), new Password("Password123@")),
        };

        var usersDto = new List<UserDto>
        {
            new() { Id = users[0].Id, Name = "UserOne" },
            new() { Id = users[1].Id, Name = "UserTwo" }
        };

        var serviceMock = new Mock<IUserService>();
        serviceMock
            .Setup(s => s.GetAllUsersAsync(cancellationToken))
            .ReturnsAsync(users);

        var mapperMock = new Mock<IMapper>();
        mapperMock
            .Setup(m => m.Map<List<UserDto>>(users))
            .Returns(usersDto);

        var handler = new GetUsersHandler(serviceMock.Object, mapperMock.Object);
        var query = new GetUsersQuery();

        // Act
        var result = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(usersDto, result);

        serviceMock.Verify(
            s => s.GetAllUsersAsync(cancellationToken),
            Times.Once
        );

        mapperMock.Verify(
            m => m.Map<List<UserDto>>(users),
            Times.Once
        );
    }
}