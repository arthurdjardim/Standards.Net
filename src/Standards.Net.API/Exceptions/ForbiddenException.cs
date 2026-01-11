using Microsoft.AspNetCore.Http;

namespace Standards.Net.API.Exceptions;

/// <summary>
/// Exception thrown when access to a resource is forbidden (HTTP 403).
/// </summary>
public class ForbiddenException : ApiException
{
    public override int StatusCode => StatusCodes.Status403Forbidden;

    public string? Resource { get; }

    public ForbiddenException(string message)
        : base(message) { }

    public ForbiddenException(string message, string resource)
        : base(message)
    {
        Resource = resource;
        AddErrorDetail("resource", resource);
    }

    public ForbiddenException()
        : base("You do not have permission to access this resource.") { }
}
