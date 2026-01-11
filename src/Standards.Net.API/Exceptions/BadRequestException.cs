using Microsoft.AspNetCore.Http;

namespace Standards.Net.API.Exceptions;

/// <summary>
/// Exception thrown when the request is malformed or invalid (HTTP 400).
/// </summary>
public class BadRequestException : ApiException
{
    public override int StatusCode => StatusCodes.Status400BadRequest;

    public BadRequestException(string message)
        : base(message) { }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException) { }
}
