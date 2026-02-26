using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;

namespace ToDo.Infrastructure.Context;

/// <summary>
/// Represents the Entity Framework Core database context for the application, providing access to entity sets and
/// configuration for the underlying database schema.
/// </summary>
/// <remarks>This context is typically configured and managed by dependency injection. Entity configurations are
/// automatically applied from the assembly containing this context. The context should be disposed of properly to
/// release database connections and resources.</remarks>
/// <param name="options">The options to be used by the DbContext. Must not be null.</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    #region DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }
    #endregion

    #region Model Configuration
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    } 
    #endregion
}