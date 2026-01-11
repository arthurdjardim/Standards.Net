using Standards.Net.API.Exceptions;
using Standards.Net.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Standards.Net.API.Filters;

/// <summary>
/// Global exception handling endpoint filter.
/// Maps common exceptions to appropriate HTTP status codes with consistent response format.
/// </summary>
public sealed class ExceptionHandlingEndpointFilter : IEndpointFilter
{
    private readonly ILogger<ExceptionHandlingEndpointFilter> _logger;

    public ExceptionHandlingEndpointFilter(ILogger<ExceptionHandlingEndpointFilter> logger)
    {
        _logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (ApiException ex)
        {
            _logger.LogWarning(ex, "API exception occurred: {Message}", ex.Message);

            var response = ApiResponse<object>.Error(ex.Message);

            if (ex.ErrorDetails is not null)
            {
                foreach (var (key, value) in ex.ErrorDetails)
                {
                    response.WithMetadata(key, value);
                }
            }

            return Results.Json(response, statusCode: ex.StatusCode);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found: {Message}", ex.Message);
            return Results.NotFound(ApiResponse<object>.Error(ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access: {Message}", ex.Message);
            return Results.Json(ApiResponse<object>.Error("Unauthorized access"), statusCode: StatusCodes.Status401Unauthorized);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation: {Message}", ex.Message);
            return Results.BadRequest(ApiResponse<object>.Error(ex.Message, new List<string> { ex.Message }));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument: {Message}", ex.Message);
            return Results.BadRequest(ApiResponse<object>.Error(ex.Message, new List<string> { ex.Message }));
        }
        catch (FormatException ex)
        {
            _logger.LogWarning(ex, "Invalid format: {Message}", ex.Message);
            return Results.BadRequest(ApiResponse<object>.Error("Invalid data format", new List<string> { ex.Message }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
            return Results.Json(
                ApiResponse<object>.Error("An unexpected error occurred. Please try again later."),
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}
