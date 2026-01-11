namespace Standards.Net.API.Options;

/// <summary>
/// Configuration options for JWT authentication.
/// </summary>
public sealed class JwtOptions
{
    /// <summary>
    /// Gets or sets the JWT issuer (who created and signed the token).
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the JWT audience (who the token is intended for).
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the secret key used to sign and validate tokens.
    /// Must be at least 32 characters for HS256 algorithm.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token expiration time in minutes.
    /// Default: 60 minutes (1 hour).
    /// </summary>
    public int ExpirationMinutes { get; set; } = 60;

    /// <summary>
    /// Gets or sets whether to validate the token issuer.
    /// Default: true.
    /// </summary>
    public bool ValidateIssuer { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to validate the token audience.
    /// Default: true.
    /// </summary>
    public bool ValidateAudience { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to validate the token lifetime.
    /// Default: true.
    /// </summary>
    public bool ValidateLifetime { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to validate the signing key.
    /// Default: true.
    /// </summary>
    public bool ValidateIssuerSigningKey { get; set; } = true;

    /// <summary>
    /// Gets or sets the clock skew to allow for time differences between systems.
    /// Default: 0 (no tolerance).
    /// </summary>
    public TimeSpan ClockSkew { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// Gets or sets the claim type for the tenant ID in multi-tenant applications.
    /// Default: "tenant_id".
    /// </summary>
    public string TenantIdClaimType { get; set; } = "tenant_id";

    /// <summary>
    /// Gets or sets the claim type for the user ID.
    /// Default: "user_id" (falls back to "sub" if not found).
    /// </summary>
    public string UserIdClaimType { get; set; } = "user_id";
}
