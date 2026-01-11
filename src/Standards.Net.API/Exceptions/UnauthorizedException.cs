using Microsoft.AspNetCore.Http;

namespace Standards.Net.API.Exceptions;

/// <summary>
/// Exception thrown when authentication is required but not provided or invalid (HTTP 401).
/// </summary>
public class UnauthorizedException : ApiException
{
    public override int StatusCode => StatusCodes.Status401Unauthorized;

    public UnauthorizedException(string message)
        : base(message) { }

    public UnauthorizedException()
        : base("Authentication is required to access this resource.") { }
}
