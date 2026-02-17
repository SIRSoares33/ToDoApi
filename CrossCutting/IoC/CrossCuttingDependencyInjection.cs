using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Psalms.AspNetCore.Auth.Jwt;
using Psalms.Auth.Jwt;
using ToDo.Application.Features.Auth.Handlers;
using ToDo.Application.Features.Auth.Validations;
using ToDo.Application.Interfaces;
using ToDo.Application.Mappings;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;
using ToDo.Domain.ValueObjects;
using ToDo.Infrastructure.Context;
using ToDo.Infrastructure.Services;

namespace ToDo.CrossCutting.IoC;

public static class CrossCuttingDependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Psalms
        services.AddScoped<PsalmsJwtTokenService>();
        services.AddPsalmsJwtAuthentication(configuration);

        // mediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly));

        // fluent validation
        services.AddFluentValidationAutoValidation(opt =>
        {
            opt.DisableDataAnnotationsValidation = true;
        });

        services.AddValidatorsFromAssemblyContaining<LoginValidation>();

        // AutoMapper
        services.AddAutoMapper(cfg => cfg.AddProfile<UserProfile>());

        // Logs
        services.AddLogging(config =>
        {
            config.AddConsole();
            config.SetMinimumLevel(LogLevel.Information);
        });

        services.AddScoped<ILogger<AuthService>, Logger<AuthService>>();

        return services;
    }

    public static async Task SeedDbAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (await context.Users.AnyAsync()) return;

        context.Database.Migrate();

        var service = scope.ServiceProvider.GetRequiredService<IAuthService>();

        await service.RegisterAsync
        (
            new User
            (
                new Name("Admin"),
                new Email("admin@email.com"),
                new Password("Admin1234")
            ), Role.Admin, CancellationToken.None
        );
    }
}