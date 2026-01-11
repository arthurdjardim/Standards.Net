using Microsoft.AspNetCore.Http;

namespace Standards.Net.API.Exceptions;

/// <summary>
/// Exception thrown when validation fails (HTTP 400).
/// </summary>
public class ValidationException : ApiException
{
    public override int StatusCode => StatusCodes.Status400BadRequest;

    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
        AddErrorDetail("validationErrors", errors);
    }

    public ValidationException(string field, string error)
        : base($"Validation failed for {field}: {error}")
    {
        Errors = new Dictionary<string, string[]> { [field] = new[] { error } };
        AddErrorDetail("validationErrors", Errors);
    }

    public ValidationException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }
}
