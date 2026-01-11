namespace Standards.Net.Exceptions;

/// <summary>
/// Base exception for all domain-specific errors.
/// Provides a foundation for standardized error handling without HTTP dependencies.
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>
    /// Gets the error code associated with this exception.
    /// Used to map to appropriate HTTP status codes in the API layer.
    /// </summary>
    public abstract ErrorCode Code { get; }

    /// <summary>
    /// Gets additional error details that can be included in the response.
    /// </summary>
    public Dictionary<string, object>? ErrorDetails { get; protected set; }

    protected DomainException(string message)
        : base(message) { }

    protected DomainException(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Adds custom error details to the exception.
    /// </summary>
    public void AddErrorDetail(string key, object value)
    {
        ErrorDetails ??= new Dictionary<string, object>();
        ErrorDetails[key] = value;
    }
}
