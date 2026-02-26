namespace ToDo.Domain.ValueObjects.Task;

public class Description
{
    #region Value
    public string? Value { get; private set; }
    #endregion

    #region Constructors
    public Description() {}
    public Description(string? value)
    {
        if (string.IsNullOrEmpty(value)) return;

        if (value.Length > 500)
            throw new ArgumentException("Description cannot exceed 500 characters.");

        Value = value;
    }
    #endregion
}