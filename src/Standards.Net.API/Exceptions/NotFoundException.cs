using Microsoft.AspNetCore.Http;

namespace Standards.Net.API.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found (HTTP 404).
/// </summary>
public class NotFoundException : ApiException
{
    public override int StatusCode => StatusCodes.Status404NotFound;

    public string ResourceType { get; }
    public object ResourceId { get; }

    public NotFoundException(string resourceType, object resourceId)
        : base($"{resourceType} with ID '{resourceId}' was not found.")
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
        AddErrorDetail("resourceType", resourceType);
        AddErrorDetail("resourceId", resourceId);
    }

    public NotFoundException(string message)
        : base(message)
    {
        ResourceType = "Resource";
        ResourceId = "Unknown";
    }
}
