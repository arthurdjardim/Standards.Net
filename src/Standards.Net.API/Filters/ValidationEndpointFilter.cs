using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Standards.Net.API.Models;

namespace Standards.Net.API.Filters;

/// <summary>
/// Endpoint filter for automatic command/query validation using FluentValidation.
/// Intercepts requests and validates the input before the handler executes.
/// </summary>
/// <typeparam name="TRequest">The type of request object to validate.</typeparam>
public sealed class ValidationEndpointFilter<TRequest> : IEndpointFilter
    where TRequest : class
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationEndpointFilter(IServiceProvider serviceProvider)
    {
        _validator = serviceProvider.GetService<IValidator<TRequest>>();
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        if (_validator is null)
        {
            return await next(context);
        }

        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();
        if (request is null)
        {
            return await next(context);
        }

        var validationResult = await _validator.ValidateAsync(
            request,
            context.HttpContext.RequestAborted
        );

        if (!validationResult.IsValid)
        {
            var errors = validationResult
                .Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            // Return just error messages without field names for backward compatibility
            var errorMessages = errors.SelectMany(kv => kv.Value).ToList();
            var response = ApiResponse<object>.Error("Validation failed", errorMessages);

            return Results.BadRequest(response);
        }

        return await next(context);
    }
}
