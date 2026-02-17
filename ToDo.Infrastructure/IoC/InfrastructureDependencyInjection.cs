using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Application.Interfaces;
using ToDo.Domain.Interfaces.Repository;
using ToDo.Infrastructure.Context;
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

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
