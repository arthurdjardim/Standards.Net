namespace Standards.Net.Exceptions;

/// <summary>
/// Error codes that map to HTTP status codes.
/// Provides a transport-agnostic way to categorize errors.
/// </summary>
public enum ErrorCode
{
    /// <summary>
    /// Bad request - validation or malformed request (HTTP 400).
    /// </summary>
    BadRequest = 400,

    /// <summary>
    /// Unauthorized - authentication required (HTTP 401).
    /// </summary>
    Unauthorized = 401,

    /// <summary>
    /// Forbidden - access denied (HTTP 403).
    /// </summary>
    Forbidden = 403,

    /// <summary>
    /// Not found - resource does not exist (HTTP 404).
    /// </summary>
    NotFound = 404,

    /// <summary>
    /// Conflict - resource already exists or state conflict (HTTP 409).
    /// </summary>
    Conflict = 409,

    /// <summary>
    /// Unprocessable entity - semantic errors (HTTP 422).
    /// </summary>
    UnprocessableEntity = 422,

    /// <summary>
    /// Internal server error (HTTP 500).
    /// </summary>
    InternalError = 500,
}
