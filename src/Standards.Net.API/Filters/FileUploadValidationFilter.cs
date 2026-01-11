using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Standards.Net.API.Models;
using Standards.Net.API.Options;

namespace Standards.Net.API.Filters;

/// <summary>
/// Endpoint filter to validate file upload presence and size in multipart/form-data requests.
/// </summary>
public sealed class FileUploadValidationFilter : IEndpointFilter
{
    private readonly FileUploadOptions _options;

    public FileUploadValidationFilter(IOptions<FileUploadOptions> options)
    {
        _options = options.Value;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var httpContext = context.HttpContext;

        // Validate content type
        if (!httpContext.Request.HasFormContentType)
        {
            return Results.BadRequest(ApiResponse<object>.Error("No file was uploaded."));
        }

        // Access form and validate file presence - handle potential parsing errors
        IFormFileCollection files;
        try
        {
            var form = httpContext.Request.Form;
            files = form.Files;

            if (files.Count == 0)
            {
                return Results.BadRequest(ApiResponse<object>.Error("No file was uploaded."));
            }
        }
        catch (InvalidDataException)
        {
            return Results.BadRequest(ApiResponse<object>.Error("Invalid multipart form data."));
        }

        var file = files[0];

        // Validate file size
        if (file.Length > _options.MaxFileSizeBytes)
        {
            var maxSizeMb = _options.MaxFileSizeBytes / (1024.0 * 1024.0);
            return Results.BadRequest(
                ApiResponse<object>.Error(
                    $"File size exceeds maximum allowed size of {maxSizeMb:F1}MB."
                )
            );
        }

        // Validate minimum file size if configured
        if (_options.MinFileSizeBytes > 0 && file.Length < _options.MinFileSizeBytes)
        {
            var minSizeKb = _options.MinFileSizeBytes / 1024.0;
            return Results.BadRequest(
                ApiResponse<object>.Error(
                    $"File size is below minimum required size of {minSizeKb:F1}KB."
                )
            );
        }

        return await next(context);
    }
}
