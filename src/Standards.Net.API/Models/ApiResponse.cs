namespace Standards.Net.API.Models;

/// <summary>
/// Standardized API response wrapper for all endpoints.
/// Provides consistent structure for success and error responses.
/// </summary>
/// <typeparam name="T">The type of data being returned.</typeparam>
public sealed class ApiResponse<T>
{
    /// <summary>
    /// Gets or sets a value indicating whether the request was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the response data.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets a message describing the result.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets a list of validation or error messages.
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Gets or sets additional metadata about the response.
    /// </summary>
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// Creates a successful response with data.
    /// </summary>
    /// <param name="data">The response data.</param>
    /// <param name="message">Optional success message.</param>
    /// <returns>A successful API response.</returns>
    public static ApiResponse<T> Ok(T data, string? message = null) =>
        new()
        {
            Success = true,
            Data = data,
            Message = message,
        };

    /// <summary>
    /// Creates an error response with message and optional error details.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">Optional list of detailed error messages.</param>
    /// <returns>An error API response.</returns>
    public static ApiResponse<T> Error(string message, List<string>? errors = null) =>
        new()
        {
            Success = false,
            Message = message,
            Errors = errors,
        };

    /// <summary>
    /// Creates an error response with a dictionary of validation errors.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="validationErrors">Dictionary of field names and their error messages.</param>
    /// <returns>An error API response.</returns>
    public static ApiResponse<T> ValidationError(
        string message,
        IDictionary<string, string[]> validationErrors
    )
    {
        var errors = new List<string>();
        foreach (var (field, fieldErrors) in validationErrors)
        {
            errors.AddRange(fieldErrors.Select(error => $"{field}: {error}"));
        }

        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors,
            Metadata = new Dictionary<string, object> { ["validationErrors"] = validationErrors },
        };
    }

    /// <summary>
    /// Adds metadata to the response.
    /// </summary>
    /// <param name="key">The metadata key.</param>
    /// <param name="value">The metadata value.</param>
    /// <returns>The current ApiResponse instance for chaining.</returns>
    public ApiResponse<T> WithMetadata(string key, object value)
    {
        Metadata ??= new Dictionary<string, object>();
        Metadata[key] = value;
        return this;
    }
}

/// <summary>
/// Non-generic API response for endpoints that don't return data.
/// Provides factory methods for common response scenarios without returning data payloads.
/// </summary>
public static class ApiResponse
{
    /// <summary>
    /// Creates a successful response without data.
    /// </summary>
    /// <param name="message">Optional success message.</param>
    /// <returns>A successful API response.</returns>
    public static ApiResponse<object> Ok(string? message = null) =>
        new() { Success = true, Message = message };

    /// <summary>
    /// Creates an error response without data.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">Optional list of detailed error messages.</param>
    /// <returns>An error API response.</returns>
    public static ApiResponse<object> Error(string message, List<string>? errors = null) =>
        new()
        {
            Success = false,
            Message = message,
            Errors = errors,
        };
}
