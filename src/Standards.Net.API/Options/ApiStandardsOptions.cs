namespace Standards.Net.API.Options;

/// <summary>
/// Main configuration options for the Default API Standards library.
/// </summary>
public sealed class ApiStandardsOptions
{
    /// <summary>
    /// Gets or sets whether multi-tenancy support is enabled.
    /// Default: false.
    /// </summary>
    public bool EnableMultiTenancy { get; set; } = false;

    /// <summary>
    /// Gets or sets whether request context tracking is enabled.
    /// Default: true.
    /// </summary>
    public bool EnableRequestContext { get; set; } = true;

    /// <summary>
    /// Gets or sets whether correlation ID tracking is enabled.
    /// Default: true.
    /// </summary>
    public bool EnableCorrelationId { get; set; } = true;

    /// <summary>
    /// Gets or sets the header name for the correlation ID.
    /// Default: "X-Correlation-Id".
    /// </summary>
    public string CorrelationIdHeader { get; set; } = "X-Correlation-Id";

    /// <summary>
    /// Gets or sets the header name for the tenant ID (when multi-tenancy is enabled).
    /// Default: "X-Tenant-Id".
    /// </summary>
    public string TenantIdHeader { get; set; } = "X-Tenant-Id";

    /// <summary>
    /// Gets or sets whether to validate tenant ID from JWT matches the header (when multi-tenancy is enabled).
    /// Default: true.
    /// </summary>
    public bool ValidateTenantFromJwt { get; set; } = true;

    /// <summary>
    /// Gets or sets whether OpenAPI documentation is enabled.
    /// Default: true.
    /// </summary>
    public bool EnableOpenApi { get; set; } = true;

    /// <summary>
    /// Gets or sets whether CORS is enabled.
    /// Default: false.
    /// </summary>
    public bool EnableCors { get; set; } = false;

    /// <summary>
    /// Gets or sets whether health checks are enabled.
    /// Default: false.
    /// </summary>
    public bool EnableHealthChecks { get; set; } = false;

    /// <summary>
    /// Gets or sets whether rate limiting is enabled.
    /// Default: false.
    /// </summary>
    public bool EnableRateLimiting { get; set; } = false;

    /// <summary>
    /// Gets or sets whether security headers middleware is enabled.
    /// Default: true.
    /// </summary>
    public bool EnableSecurityHeaders { get; set; } = true;

    /// <summary>
    /// Gets or sets whether response compression is enabled.
    /// Default: true.
    /// </summary>
    public bool EnableResponseCompression { get; set; } = true;

    /// <summary>
    /// Gets or sets whether structured request/response logging is enabled.
    /// Default: false.
    /// </summary>
    public bool EnableRequestLogging { get; set; } = false;

    /// <summary>
    /// Gets or sets the JWT configuration options.
    /// </summary>
    public JwtOptions Jwt { get; set; } = new();

    /// <summary>
    /// Gets or sets the CORS configuration options.
    /// </summary>
    public CorsOptions Cors { get; set; } = new();

    /// <summary>
    /// Gets or sets the OpenAPI configuration options.
    /// </summary>
    public OpenApiOptions OpenApi { get; set; } = new();

    /// <summary>
    /// Gets or sets the file upload configuration options.
    /// </summary>
    public FileUploadOptions FileUpload { get; set; } = new();
}
