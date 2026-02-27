using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Psalms.AspNetCore.Auth.Jwt;
using Psalms.Auth.Jwt;
using ToDo.Application.Interfaces;
using ToDo.Domain.Interfaces.Repository;
using ToDo.Infrastructure.Context;
using ToDo.Infrastructure.Extensions;
using ToDo.Infrastructure.Repositories;
using ToDo.Infrastructure.Services;

namespace ToDo.Infrastructure.IoC;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Context
        services.AddDbContext<AppDbContext>(x => x.UseNpgsql(configuration["DatabaseConnection"]));

        // Hasher
        services.AddScoped<IPasswordHasher<IAuthService>, PasswordHasher<IAuthService>>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITodoRepository, TodoRepository>();

        // Services

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITodoService, TodoService>();

        // Extensions
        services.AddHttpContextAccessor();
        services.AddScoped<IUserClaims, UserClaims>();

        // Psalms
        services.AddPsalmsJwtAuthentication(configuration);
        services.AddScoped<PsalmsJwtTokenService>();

        return services;
    }
}
