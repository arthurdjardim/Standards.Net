using Standards.Net.API.Models;
using Standards.Net.API.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Standards.Net.API.Filters;

/// <summary>
/// Endpoint filter to validate image file extensions.
/// </summary>
public sealed class ImageExtensionValidationFilter : IEndpointFilter
{
    private readonly FileUploadOptions _options;

    public ImageExtensionValidationFilter(IOptions<FileUploadOptions> options)
    {
        _options = options.Value;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;

        if (httpContext.Request.HasFormContentType && httpContext.Request.Form.Files.Count > 0)
        {
            var file = httpContext.Request.Form.Files[0];

            // Validate file extension
            var fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            if (string.IsNullOrEmpty(fileExtension) || !_options.AllowedImageExtensions.Contains(fileExtension))
            {
                var allowedExtensionsText = string.Join(", ", _options.AllowedImageExtensions);
                return Results.BadRequest(ApiResponse<object>.Error($"Invalid file extension. Only {allowedExtensionsText} are allowed."));
            }
        }

        return await next(context);
    }
}
