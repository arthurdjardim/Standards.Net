using Microsoft.AspNetCore.Http;

namespace Standards.Net.API.Exceptions;

/// <summary>
/// Exception thrown when a resource conflict occurs (HTTP 409).
/// </summary>
public class ConflictException : ApiException
{
    public override int StatusCode => StatusCodes.Status409Conflict;

    public string ResourceType { get; }
    public object ConflictingValue { get; }

    public ConflictException(string resourceType, object conflictingValue)
        : base($"{resourceType} with value '{conflictingValue}' already exists.")
    {
        ResourceType = resourceType;
        ConflictingValue = conflictingValue;
        AddErrorDetail("resourceType", resourceType);
        AddErrorDetail("conflictingValue", conflictingValue);
    }

    public ConflictException(string message)
        : base(message)
    {
        ResourceType = "Resource";
        ConflictingValue = "Unknown";
    }
}
