using Microsoft.AspNetCore.Http;

namespace Standards.Net.API.Exceptions;

/// <summary>
/// Base exception for API-specific errors that provides standardized error handling.
/// </summary>
public abstract class ApiException : Exception
{
    /// <summary>
    /// Gets the HTTP status code associated with this exception.
    /// </summary>
    public abstract int StatusCode { get; }

    /// <summary>
    /// Gets additional error details that can be included in the response.
    /// </summary>
    public Dictionary<string, object>? ErrorDetails { get; protected set; }

    protected ApiException(string message)
        : base(message) { }

    protected ApiException(string message, Exception innerException)
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
