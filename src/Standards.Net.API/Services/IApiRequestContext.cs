namespace Standards.Net.API.Services;

/// <summary>
/// Provides access to the current API request context including tenant, user, and correlation information.
/// </summary>
public interface IApiRequestContext
{
    /// <summary>
    /// Gets the tenant ID for the current request (for multi-tenant applications).
    /// Returns null if multi-tenancy is not enabled or no tenant context is available.
    /// </summary>
    string? TenantId { get; }

    /// <summary>
    /// Gets the authenticated user ID for the current request.
    /// Returns null if the request is anonymous or no user context is available.
    /// </summary>
    string? UserId { get; }

    /// <summary>
    /// Gets the correlation ID for the current request for tracking across services.
    /// A new correlation ID is generated if one is not provided in the request headers.
    /// </summary>
    string CorrelationId { get; }

    /// <summary>
    /// Gets the IP address of the client making the request.
    /// </summary>
    string? IpAddress { get; }

    /// <summary>
    /// Gets the user agent string from the request.
    /// </summary>
    string? UserAgent { get; }

    /// <summary>
    /// Sets the tenant ID for the current request.
    /// </summary>
    /// <param name="tenantId">The tenant ID to set.</param>
    void SetTenantId(string? tenantId);

    /// <summary>
    /// Sets the user ID for the current request.
    /// </summary>
    /// <param name="userId">The user ID to set.</param>
    void SetUserId(string? userId);

    /// <summary>
    /// Sets the correlation ID for the current request.
    /// </summary>
    /// <param name="correlationId">The correlation ID to set.</param>
    void SetCorrelationId(string correlationId);

    /// <summary>
    /// Sets the IP address for the current request.
    /// </summary>
    /// <param name="ipAddress">The IP address to set.</param>
    void SetIpAddress(string? ipAddress);

    /// <summary>
    /// Sets the user agent for the current request.
    /// </summary>
    /// <param name="userAgent">The user agent to set.</param>
    void SetUserAgent(string? userAgent);

    /// <summary>
    /// Gets a custom value from the request context.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The key to retrieve.</param>
    /// <returns>The value if found; otherwise, default(T).</returns>
    T? GetValue<T>(string key);

    /// <summary>
    /// Sets a custom value in the request context.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The key to store the value under.</param>
    /// <param name="value">The value to store.</param>
    void SetValue<T>(string key, T value);

    /// <summary>
    /// Clears all context data (called at the end of a request).
    /// </summary>
    void Clear();
}
