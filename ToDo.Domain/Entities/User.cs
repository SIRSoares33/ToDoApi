using System.ComponentModel.DataAnnotations;
using ToDo.Domain.Enums;
using ToDo.Domain.ValueObjects;

namespace ToDo.Domain.Entities;

public class User
{
    #region Attributes
    [Key]
    public Guid Id { get; private set; }
    public Name Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Password HashPassword { get; private set; } = null!;
    public Role Role { get; private set; }
    #endregion

    #region Constructors
    private User() { } // For ORM

    public User(Name name, Email email, Password password)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        HashPassword = password;
    }

    public User(Name name, Email email, Password password, Role role) : this(name, email, password)
        => Role = role;
    #endregion

    #region Methods to Update Value Objects
    public void ChangeName(Name name) => Name = name;
    public void ChangeEmail(Email email) => Email = email;
    public void ChangePassword(Password password) => HashPassword = password;
    public void ChangeRole(Role role) => Role = role;

    public void Update(string? name, string? email, string? hashedPassword, Role? role)
    {
        if (!string.IsNullOrEmpty(name) || !string.IsNullOrWhiteSpace(name)) ChangeName(new Name(name));

        if (!string.IsNullOrEmpty(email) || !string.IsNullOrWhiteSpace(email)) ChangeEmail(new Email(email));

        if (!string.IsNullOrEmpty(hashedPassword) || !string.IsNullOrWhiteSpace(hashedPassword)) ChangePassword(new Password(hashedPassword));

        if (role.HasValue) ChangeRole(role.Value);
    }
    #endregion
}