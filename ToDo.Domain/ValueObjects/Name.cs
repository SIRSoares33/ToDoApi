using ToDo.Domain.Exceptions;

namespace ToDo.Domain.ValueObjects;

public class Name
{
    #region Value Object Implementation
    public string Value { get; } = null!;
    #endregion

    #region Constructor
    public Name() {}
    public Name(string? name)
    {
        Validate(name);

        Value = name!.Trim();
    }
    #endregion

    #region Validation
    private static void Validate(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be null or empty.");
        
        if (name.Length > 100)
            throw new DomainException("Name cannot exceed 100 characters.");

        if (name.Length < 3)
            throw new DomainException("Name must be at least 3 characters long.");

        if (name.Any(char.IsDigit))
            throw new DomainException("Name cannot contain numbers.");

        if (name.Any(char.IsPunctuation))
            throw new DomainException("Name cannot contain punctuation characters.");

        if (name.Any(char.IsControl))
            throw new DomainException("Name cannot contain control characters.");

        // Additional validation rules can be added here (e.g., allowed characters, etc.)
    }
    #endregion
}