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
    public Role? Role { get; private set; }
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

    public void Update(User user)
    {
        if (!string.IsNullOrEmpty(user.Name.Value)) ChangeName(new Name(user.Name.Value));

        if (!string.IsNullOrEmpty(user.Email.Value)) ChangeEmail(new Email(user.Email.Value));

        if (!string.IsNullOrEmpty(user.HashPassword.Value)) ChangePassword(new Password(user.HashPassword.Value));

        if (user.Role is not null && user.Role.Value != Role!.Value) ChangeRole(user.Role.Value);
    }
    #endregion
}