using ToDo.Domain.Exceptions;

namespace ToDo.Domain.ValueObjects.Users;

public class Password
{
    #region Value Object Implementation
    public string Value { get; } = null!;
    #endregion

    #region Constructors
    public Password() {} // EF
    public Password(string? password)
    {
        Validate(password);
        Value = password!.Trim();
    }
    #endregion

    #region Validation
    private static void Validate(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new DomainException("Password cannot be null or empty.");

        if (password.Length < 8)
            throw new DomainException("Password must be at least 8 characters long.");

        if (!password.Any(char.IsUpper))
            throw new DomainException("Password must contain at least one uppercase letter.");

        if (!password.Any(char.IsLower))
            throw new DomainException("Password must contain at least one lowercase letter.");

        if (!password.Any(char.IsDigit))
            throw new DomainException("Password must contain at least one digit.");

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            throw new DomainException("Password must contain at least one special character.");
    }
    #endregion
}
