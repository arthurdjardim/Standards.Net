using Standards.Net.API.Options;
using Standards.Net.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Standards.Net.API.Middleware;

/// <summary>
/// Middleware that populates the IApiRequestContext with tenant ID, user ID, correlation ID, and other request information.
/// This middleware should be registered early in the pipeline to ensure context is available for all subsequent middleware and endpoints.
/// </summary>
public sealed class RequestContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestContextMiddleware> _logger;
    private readonly ApiStandardsOptions _options;

    public RequestContextMiddleware(RequestDelegate next, ILogger<RequestContextMiddleware> logger, IOptions<ApiStandardsOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context, IApiRequestContext requestContext)
    {
        try
        {
            // Extract and set correlation ID
            if (_options.EnableCorrelationId)
            {
                var correlationId = context.Request.Headers[_options.CorrelationIdHeader].FirstOrDefault() ?? Guid.NewGuid().ToString();
                requestContext.SetCorrelationId(correlationId);
                context.Response.Headers.Append(_options.CorrelationIdHeader, correlationId);
                _logger.LogDebug("Correlation ID set to {CorrelationId}", correlationId);
            }

            // Extract and set tenant ID (if multi-tenancy is enabled)
            if (_options.EnableMultiTenancy)
            {
                var tenantId = ExtractTenantId(context);
                if (!string.IsNullOrEmpty(tenantId))
                {
                    requestContext.SetTenantId(tenantId);
                    _logger.LogDebug("Tenant ID set to {TenantId}", tenantId);
                }
                else
                {
                    _logger.LogDebug("No Tenant ID found in request");
                }
            }

            // Extract and set user ID
            var userId = ExtractUserId(context);
            if (!string.IsNullOrEmpty(userId))
            {
                requestContext.SetUserId(userId);
                _logger.LogDebug("User ID set to {UserId}", userId);
            }
            else
            {
                _logger.LogDebug("No User ID found in request");
            }

            // Set IP address
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            if (!string.IsNullOrEmpty(ipAddress))
            {
                requestContext.SetIpAddress(ipAddress);
            }

            // Set User Agent
            var userAgent = context.Request.Headers.UserAgent.FirstOrDefault();
            if (!string.IsNullOrEmpty(userAgent))
            {
                requestContext.SetUserAgent(userAgent);
            }

            // Store route values
            foreach (var routeValue in context.Request.RouteValues)
            {
                if (routeValue.Value is not null)
                {
                    requestContext.SetValue($"Route_{routeValue.Key}", routeValue.Value.ToString()!);
                }
            }

            await _next(context);
        }
        finally
        {
            requestContext.Clear();
        }
    }

    private string? ExtractTenantId(HttpContext context)
    {
        // Try to get from JWT claim first
        var tenantIdClaim = context.User.FindFirst(_options.Jwt.TenantIdClaimType)?.Value;
        if (!string.IsNullOrEmpty(tenantIdClaim))
        {
            return tenantIdClaim;
        }

        // Fall back to header
        var tenantIdHeader = context.Request.Headers[_options.TenantIdHeader].FirstOrDefault();
        if (!string.IsNullOrEmpty(tenantIdHeader))
        {
            return tenantIdHeader;
        }

        return null;
    }

    private string? ExtractUserId(HttpContext context)
    {
        // Try configured user ID claim type first
        var userIdClaim = context.User.FindFirst(_options.Jwt.UserIdClaimType)?.Value;
        if (!string.IsNullOrEmpty(userIdClaim))
        {
            return userIdClaim;
        }

        // Fall back to standard "sub" claim
        var subClaim = context.User.FindFirst("sub")?.Value;
        if (!string.IsNullOrEmpty(subClaim))
        {
            return subClaim;
        }

        return null;
    }
}
