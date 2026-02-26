using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net;
using System.Net.Http.Json;
using ToDo.Api.Tests.Fakers;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Features.Queries.Users;
using ToDo.Application.Interfaces;

namespace ToDo.Api.Tests.Controllers;

public class UsersControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IUserClaims> _userClaimsMock;

    public UsersControllerTests(WebApplicationFactory<Program> factory)
    {
        _mediatorMock = new Mock<IMediator>();
        _userClaimsMock = new Mock<IUserClaims>();

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(IMediator));
                services.RemoveAll(typeof(IUserClaims));

                services.AddSingleton(_mediatorMock.Object);
                services.AddSingleton(_userClaimsMock.Object);

                // Auth Fake
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = FakeAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = FakeAuthHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>(
                    FakeAuthHandler.SchemeName, _ => { });
            });
        }).CreateClient();
    }

    // ============================
    // GET /api/users (Admin)
    // ============================
    [Fact]
    public async Task GetUsers_ShouldReturnOk()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var response = await _client.GetAsync("/api/users");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // ============================
    // PUT /api/users/{id} (Admin)
    // ============================
    [Fact]
    public async Task UpdateUserByAdmin_ShouldReturnNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var dto = new UpdateUserDto
        {
            Name = "Admin Update",
            Email = "admin@email.com"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/users/{userId}", dto);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        _mediatorMock.Verify(m =>
            m.Send(It.IsAny<UpdateUserByAdminCommand>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ============================
    // PUT /api/users (Own Account)
    // ============================
    [Fact]
    public async Task UpdateOwnAccount_ShouldReturnNoContent()
    {
        // Arrange
        _userClaimsMock
            .Setup(u => u.UserId)
            .Returns(Guid.NewGuid());

        var dto = new UpdateUserDto
        {
            Name = "User Update",
            Email = "user@email.com"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/users", dto);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        _mediatorMock.Verify(m =>
            m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ============================
    // DELETE /api/users/{id} (Admin)
    // ============================
    [Fact]
    public async Task DeleteUserByAdmin_ShouldReturnNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/users/{userId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        _mediatorMock.Verify(m =>
            m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ============================
    // DELETE /api/users (Own Account)
    // ============================
    [Fact]
    public async Task DeleteOwnAccount_ShouldReturnNoContent()
    {
        // Arrange
        _userClaimsMock
            .Setup(u => u.UserId)
            .Returns(Guid.NewGuid());

        // Act
        var response = await _client.DeleteAsync("/api/users");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        _mediatorMock.Verify(m =>
            m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}