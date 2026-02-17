using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Entities;
using ToDo.Domain.Enums;
using ToDo.Domain.ValueObjects;
namespace ToDo.Infrastructure.Configuration;

/// <summary>
/// Configures the entity mapping for the User type in the Entity Framework model.
/// </summary>
/// <remarks>This configuration defines the table name, primary key, and owned properties for the User entity. It
/// is intended to be used with Entity Framework Core's model building infrastructure to ensure consistent mapping
/// between the User class and the underlying database schema.</remarks>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.OwnsOne(u => u.Name, name => name.Property(n => n.Value).HasColumnName("Name").IsRequired());
        builder.OwnsOne(u => u.Email, email => email.Property(e => e.Value).HasColumnName("Email").IsRequired());
        builder.OwnsOne(u => u.HashPassword, password => password.Property(p => p.Value).HasColumnName("Password").IsRequired());
    }
}