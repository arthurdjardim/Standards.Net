using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Standards.Net.API.Services;

namespace Standards.Net.API.Middleware;

/// <summary>
/// Middleware that logs HTTP requests and responses with structured logging.
/// Includes correlation IDs, timing information, and request/response details.
/// </summary>
public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IApiRequestContext requestContext)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;
        var correlationId = requestContext.CorrelationId;

        _logger.LogInformation(
            "HTTP {Method} {Path} started. CorrelationId: {CorrelationId}, TenantId: {TenantId}, UserId: {UserId}, IP: {IpAddress}",
            requestMethod,
            requestPath,
            correlationId,
            requestContext.TenantId ?? "N/A",
            requestContext.UserId ?? "Anonymous",
            requestContext.IpAddress ?? "Unknown"
        );

        try
        {
            await _next(context);

            stopwatch.Stop();

            var statusCode = context.Response.StatusCode;
            var elapsedMs = stopwatch.ElapsedMilliseconds;

            if (statusCode >= 200 && statusCode < 400)
            {
                _logger.LogInformation(
                    "HTTP {Method} {Path} completed with {StatusCode} in {ElapsedMs}ms. CorrelationId: {CorrelationId}",
                    requestMethod,
                    requestPath,
                    statusCode,
                    elapsedMs,
                    correlationId
                );
            }
            else if (statusCode >= 400 && statusCode < 500)
            {
                _logger.LogWarning(
                    "HTTP {Method} {Path} completed with {StatusCode} in {ElapsedMs}ms. CorrelationId: {CorrelationId}",
                    requestMethod,
                    requestPath,
                    statusCode,
                    elapsedMs,
                    correlationId
                );
            }
            else
            {
                _logger.LogError(
                    "HTTP {Method} {Path} completed with {StatusCode} in {ElapsedMs}ms. CorrelationId: {CorrelationId}",
                    requestMethod,
                    requestPath,
                    statusCode,
                    elapsedMs,
                    correlationId
                );
            }
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "HTTP {Method} {Path} failed after {ElapsedMs}ms. CorrelationId: {CorrelationId}",
                requestMethod,
                requestPath,
                stopwatch.ElapsedMilliseconds,
                correlationId
            );

            throw;
        }
    }
}
