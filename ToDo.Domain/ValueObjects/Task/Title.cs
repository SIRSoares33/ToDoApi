using ToDo.Domain.Exceptions;

namespace ToDo.Domain.ValueObjects.Task;

public class Title
{
    #region Value
    public string Value { get; private set; } = string.Empty;
    #endregion

    #region Constructors
    public Title() {}

    public Title(string value)
    {
        Validate(value);

        Value = value;
    }
    #endregion

    #region Validation
    private static void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Title cannot be empty.");

        if (value.Length > 100)
            throw new DomainException("Title cannot exceed 100 characters.");
    }
    #endregion
}
