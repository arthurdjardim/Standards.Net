using Scalar.AspNetCore;

namespace Standards.Net.API.Options;

/// <summary>
/// Configuration options for OpenAPI documentation.
/// </summary>
public sealed class OpenApiOptions
{
    /// <summary>
    /// Gets or sets whether OpenAPI documentation is enabled.
    /// Default: true in Development, false in Production.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the API title shown in the documentation.
    /// Default: "API Documentation".
    /// </summary>
    public string Title { get; set; } = "API Documentation";

    /// <summary>
    /// Gets or sets the API description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API version.
    /// Default: "v1".
    /// </summary>
    public string Version { get; set; } = "v1";

    /// <summary>
    /// Gets or sets the contact name for the API.
    /// </summary>
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact email for the API.
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether to use Scalar UI for API documentation.
    /// Default: true. Set to false to use Swagger UI instead.
    /// </summary>
    public bool UseScalar { get; set; } = true;

    /// <summary>
    /// Gets or sets the Scalar theme.
    /// Default: Laserwave.
    /// </summary>
    public ScalarTheme ScalarTheme { get; set; } = ScalarTheme.Laserwave;

    /// <summary>
    /// Gets or sets the route path for the OpenAPI JSON document.
    /// Default: "/openapi/v1.json".
    /// </summary>
    public string DocumentRoute { get; set; } = "/openapi/v1.json";

    /// <summary>
    /// Gets or sets the route path for the Scalar/Swagger UI.
    /// Default: "/scalar" for Scalar, "/swagger" for Swagger.
    /// </summary>
    public string UiRoute { get; set; } = "/scalar";
}
