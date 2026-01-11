namespace Standards.Net.Exceptions;

/// <summary>
/// Exception thrown when input validation fails.
/// </summary>
public class ValidationException : DomainException
{
    public override ErrorCode Code => ErrorCode.BadRequest;

    /// <summary>
    /// Gets the validation errors.
    /// </summary>
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors.ToList().AsReadOnly();
        AddErrorDetail("errors", Errors);
    }

    public ValidationException(string error)
        : base(error)
    {
        Errors = new List<string> { error }.AsReadOnly();
        AddErrorDetail("errors", Errors);
    }

    public ValidationException(string message, IEnumerable<string> errors)
        : base(message)
    {
        Errors = errors.ToList().AsReadOnly();
        AddErrorDetail("errors", Errors);
    }
}
