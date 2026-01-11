namespace Standards.Net.Exceptions;

/// <summary>
/// Exception thrown when access to a resource is forbidden.
/// </summary>
public class ForbiddenException : DomainException
{
    public override ErrorCode Code => ErrorCode.Forbidden;

    /// <summary>
    /// Gets the reason for the forbidden access, if specified.
    /// </summary>
    public string? Reason { get; }

    public ForbiddenException(string message)
        : base(message) { }

    public ForbiddenException(string message, string reason)
        : base(message)
    {
        Reason = reason;
        AddErrorDetail("reason", reason);
    }

    public ForbiddenException()
        : base("You do not have permission to access this resource.") { }
}
