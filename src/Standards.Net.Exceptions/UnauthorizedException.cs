namespace Standards.Net.Exceptions;

/// <summary>
/// Exception thrown when authentication is required but not provided or invalid.
/// </summary>
public class UnauthorizedException : DomainException
{
    public override ErrorCode Code => ErrorCode.Unauthorized;

    public UnauthorizedException()
        : base("Authentication is required to access this resource.") { }

    public UnauthorizedException(string message)
        : base(message) { }
}
