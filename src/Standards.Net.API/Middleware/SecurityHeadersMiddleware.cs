using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Standards.Net.API.Middleware;

/// <summary>
/// Middleware that adds security headers to HTTP responses to enhance application security.
/// Implements common security best practices including HSTS, CSP, X-Frame-Options, etc.
/// </summary>
public sealed class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityHeadersMiddleware> _logger;

    public SecurityHeadersMiddleware(
        RequestDelegate next,
        ILogger<SecurityHeadersMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // X-Content-Type-Options: Prevent MIME type sniffing
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

        // X-Frame-Options: Prevent clickjacking attacks
        context.Response.Headers.Append("X-Frame-Options", "DENY");

        // X-XSS-Protection: Enable XSS filter (for older browsers)
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

        // Referrer-Policy: Control referrer information
        context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

        // Content-Security-Policy: Restrict resource loading
        context.Response.Headers.Append(
            "Content-Security-Policy",
            "default-src 'self'; frame-ancestors 'none'; upgrade-insecure-requests;"
        );

        // Permissions-Policy: Control browser features and APIs
        context.Response.Headers.Append(
            "Permissions-Policy",
            "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()"
        );

        // Remove potentially sensitive server headers
        context.Response.Headers.Remove("Server");
        context.Response.Headers.Remove("X-Powered-By");
        context.Response.Headers.Remove("X-AspNet-Version");
        context.Response.Headers.Remove("X-AspNetMvc-Version");

        _logger.LogDebug("Security headers added to response");

        await _next(context);
    }
}
