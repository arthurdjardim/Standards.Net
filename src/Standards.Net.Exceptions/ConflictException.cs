namespace Standards.Net.Exceptions;

/// <summary>
/// Exception thrown when a resource conflict occurs (duplicate resource, state conflict, etc.).
/// </summary>
public class ConflictException : DomainException
{
    public override ErrorCode Code => ErrorCode.Conflict;

    /// <summary>
    /// Gets the name of the property that has a conflicting value.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the conflicting value.
    /// </summary>
    public object PropertyValue { get; }

    public ConflictException(string propertyName, object propertyValue)
        : base($"A resource with {propertyName} '{propertyValue}' already exists.")
    {
        PropertyName = propertyName;
        PropertyValue = propertyValue;
        AddErrorDetail("propertyName", propertyName);
        AddErrorDetail("propertyValue", propertyValue);
    }

    public ConflictException(string message)
        : base(message)
    {
        PropertyName = "Unknown";
        PropertyValue = "Unknown";
    }

    public ConflictException(string propertyName, object propertyValue, string message)
        : base(message)
    {
        PropertyName = propertyName;
        PropertyValue = propertyValue;
        AddErrorDetail("propertyName", propertyName);
        AddErrorDetail("propertyValue", propertyValue);
    }
}
