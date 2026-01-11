using Standards.Net.API.Models;
using Standards.Net.API.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Standards.Net.API.Middleware;

/// <summary>
/// Middleware that validates tenant context consistency between JWT claims and request headers.
/// Prevents cross-tenant access by ensuring the tenant_id in the JWT matches the X-Tenant-Id header.
/// Only active when multi-tenancy is enabled in ApiStandardsOptions.
/// </summary>
public sealed class TenantJwtValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantJwtValidationMiddleware> _logger;
    private readonly ApiStandardsOptions _options;

    public TenantJwtValidationMiddleware(RequestDelegate next, ILogger<TenantJwtValidationMiddleware> logger, IOptions<ApiStandardsOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options.Value;
    }

    /// <summary>
    /// Validates that JWT tenant_id claim matches X-Tenant-Id header to prevent cross-tenant access.
    /// Returns 403 Forbidden if authenticated user attempts to access different tenant's resources.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        // Skip validation if multi-tenancy is not enabled or validation is disabled
        if (!_options.EnableMultiTenancy || !_options.ValidateTenantFromJwt)
        {
            await _next(context);
            return;
        }

        // Only validate for authenticated requests
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var jwtTenantId = context.User.FindFirst(_options.Jwt.TenantIdClaimType)?.Value;
            var headerTenantId = context.Request.Headers[_options.TenantIdHeader].FirstOrDefault();

            // If both values exist and don't match, return 403 Forbidden
            if (
                !string.IsNullOrEmpty(jwtTenantId)
                && !string.IsNullOrEmpty(headerTenantId)
                && !string.Equals(jwtTenantId, headerTenantId, StringComparison.OrdinalIgnoreCase)
            )
            {
                _logger.LogWarning(
                    "Tenant mismatch detected. JWT tenant_id: {JwtTenantId}, Header {HeaderName}: {HeaderTenantId}",
                    jwtTenantId,
                    _options.TenantIdHeader,
                    headerTenantId
                );

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(
                    ApiResponse.Error("Tenant context mismatch. You cannot access resources from a different tenant.")
                );
                return;
            }
        }

        await _next(context);
    }
}
