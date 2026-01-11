namespace Standards.Net.API.Options;

/// <summary>
/// Configuration options for Cross-Origin Resource Sharing (CORS).
/// </summary>
public sealed class CorsOptions
{
    /// <summary>
    /// Gets or sets the name of the CORS policy.
    /// Default: "DefaultCorsPolicy".
    /// </summary>
    public string PolicyName { get; set; } = "DefaultCorsPolicy";

    /// <summary>
    /// Gets or sets the allowed origins.
    /// Use "*" to allow all origins (not recommended for production).
    /// </summary>
    public List<string> AllowedOrigins { get; set; } = new();

    /// <summary>
    /// Gets or sets whether to allow any origin.
    /// Default: false. Set to true for development only.
    /// </summary>
    public bool AllowAnyOrigin { get; set; } = false;

    /// <summary>
    /// Gets or sets the allowed HTTP methods.
    /// Default: GET, POST, PUT, DELETE, PATCH, OPTIONS.
    /// </summary>
    public List<string> AllowedMethods { get; set; } = new() { "GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS" };

    /// <summary>
    /// Gets or sets whether to allow any HTTP method.
    /// Default: false.
    /// </summary>
    public bool AllowAnyMethod { get; set; } = false;

    /// <summary>
    /// Gets or sets the allowed headers.
    /// Default: Content-Type, Authorization, X-Tenant-Id, X-Correlation-Id.
    /// </summary>
    public List<string> AllowedHeaders { get; set; } = new() { "Content-Type", "Authorization", "X-Tenant-Id", "X-Correlation-Id" };

    /// <summary>
    /// Gets or sets whether to allow any header.
    /// Default: false.
    /// </summary>
    public bool AllowAnyHeader { get; set; } = false;

    /// <summary>
    /// Gets or sets the headers to expose to the client.
    /// </summary>
    public List<string> ExposedHeaders { get; set; } = new() { "X-Correlation-Id" };

    /// <summary>
    /// Gets or sets whether to allow credentials (cookies, authorization headers).
    /// Default: true.
    /// </summary>
    public bool AllowCredentials { get; set; } = true;

    /// <summary>
    /// Gets or sets the preflight cache duration in seconds.
    /// Default: 600 seconds (10 minutes).
    /// </summary>
    public int PreflightMaxAge { get; set; } = 600;
}
