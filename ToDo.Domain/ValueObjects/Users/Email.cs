using System.Net.Mail;
using ToDo.Domain.Exceptions;

namespace ToDo.Domain.ValueObjects.Users;

public class Email
{
    #region Value Object Implementation
    public string Value { get; } = null!;
    #endregion

    #region Constructors
    private Email() {} // EF
    public Email(string? email)
    {
        Validate(email);
        Value = email!;
    }   
    #endregion

    #region Validation
    private static void Validate(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be null or empty.");
        if (email.Length > 255)
            throw new DomainException("Email cannot exceed 255 characters.");
        if (!IsValidEmail(email))
            throw new DomainException("Email format is invalid.");
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    #endregion
}
