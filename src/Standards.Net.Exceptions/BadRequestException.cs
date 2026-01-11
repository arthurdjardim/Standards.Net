namespace Standards.Net.Exceptions;

/// <summary>
/// Exception thrown when a business rule is violated or the request is invalid.
/// </summary>
public class BadRequestException : DomainException
{
    public override ErrorCode Code => ErrorCode.BadRequest;

    /// <summary>
    /// Gets the name of the business rule that was violated, if applicable.
    /// </summary>
    public string? RuleName { get; }

    public BadRequestException(string message)
        : base(message) { }

    public BadRequestException(string message, string ruleName)
        : base(message)
    {
        RuleName = ruleName;
        AddErrorDetail("ruleName", ruleName);
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException) { }
}
