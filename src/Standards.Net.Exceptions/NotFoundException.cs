namespace Standards.Net.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found.
/// </summary>
public class NotFoundException : DomainException
{
    public override ErrorCode Code => ErrorCode.NotFound;

    /// <summary>
    /// Gets the type of resource that was not found.
    /// </summary>
    public string ResourceType { get; }

    /// <summary>
    /// Gets the identifier of the resource that was not found.
    /// </summary>
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

    public NotFoundException(string resourceType, object resourceId, string message)
        : base(message)
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
        AddErrorDetail("resourceType", resourceType);
        AddErrorDetail("resourceId", resourceId);
    }
}
