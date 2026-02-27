using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using ToDo.Infrastructure.Extensions;

namespace ToDo.Infrastructure.Tests.Extensions;

public static class HttpContextFactory
{
    public static HttpContext CreateWithClaims(params Claim[] claims)
    {
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"))
        };

        return context;
    }
}

public class UserClaimsTests
{
    private static UserClaims CreateServiceWithContext(HttpContext? context)
    {
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(context);

        return new UserClaims(accessor.Object);
    }

    [Fact]
    public void UserId_Should_Return_Guid_When_Claim_Exists()
    {
        var userId = Guid.NewGuid();

        var context = HttpContextFactory.CreateWithClaims(
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

        var service = CreateServiceWithContext(context);

        Assert.Equal(userId, service.UserId);
    }

    [Fact]
    public void UserId_Should_Throw_When_Claim_Missing()
    {
        var context = HttpContextFactory.CreateWithClaims();

        var service = CreateServiceWithContext(context);

        Assert.Throws<UnauthorizedAccessException>(() => service.UserId);
    }

    [Fact]
    public void UserRole_Should_Return_Role_When_Claim_Exists()
    {
        var context = HttpContextFactory.CreateWithClaims(
            new Claim(ClaimTypes.Role, "Admin"));

        var service = CreateServiceWithContext(context);

        Assert.Equal("Admin", service.UserRole);
    }

    [Fact]
    public void UserRole_Should_Throw_When_Claim_Missing()
    {
        var context = HttpContextFactory.CreateWithClaims();

        var service = CreateServiceWithContext(context);

        Assert.Throws<KeyNotFoundException>(() => service.UserRole);
    }

    [Fact]
    public void Email_Should_Return_Email_When_Claim_Exists()
    {
        var context = HttpContextFactory.CreateWithClaims(
            new Claim(ClaimTypes.Email, "test@email.com"));

        var service = CreateServiceWithContext(context);

        Assert.Equal("test@email.com", service.Email);
    }

    [Fact]
    public void Email_Should_Throw_When_Claim_Missing()
    {
        var context = HttpContextFactory.CreateWithClaims();

        var service = CreateServiceWithContext(context);

        Assert.Throws<UnauthorizedAccessException>(() => service.Email);
    }

    [Fact]
    public void UserName_Should_Return_Name_When_Claim_Exists()
    {
        var context = HttpContextFactory.CreateWithClaims(
            new Claim(ClaimTypes.Name, "Gustavo"));

        var service = CreateServiceWithContext(context);

        Assert.Equal("Gustavo", service.UserName);
    }

    [Fact]
    public void UserName_Should_Throw_When_Claim_Missing()
    {
        var context = HttpContextFactory.CreateWithClaims();

        var service = CreateServiceWithContext(context);

        Assert.Throws<KeyNotFoundException>(() => service.UserName);
    }

    [Fact]
    public void Should_Throw_When_HttpContext_Is_Null()
    {
        var service = CreateServiceWithContext(null);

        Assert.Throws<UnauthorizedAccessException>(() => service.UserId);
        Assert.Throws<UnauthorizedAccessException>(() => service.Email);
        Assert.Throws<KeyNotFoundException>(() => service.UserRole);
        Assert.Throws<KeyNotFoundException>(() => service.UserName);
    }
}