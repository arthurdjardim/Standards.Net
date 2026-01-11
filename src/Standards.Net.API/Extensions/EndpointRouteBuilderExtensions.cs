using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Standards.Net.API.Filters;

namespace Standards.Net.API.Extensions;

/// <summary>
/// Extension methods for configuring endpoint routes with standard filters and conventions.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    extension(IEndpointRouteBuilder builder)
    {
        /// <summary>
        /// Creates a route group with standard exception handling filter applied.
        /// This is the recommended way to create route groups as it ensures consistent error handling.
        /// </summary>
        /// <param name="prefix">The route prefix for the group (e.g., "/api/v1").</param>
        /// <returns>The route group builder for further configuration.</returns>
        public RouteGroupBuilder MapApiGroup(string prefix)
        {
            return builder.MapGroup(prefix).AddEndpointFilter<ExceptionHandlingEndpointFilter>();
        }

        /// <summary>
        /// Creates a route group with standard exception handling and validation filters applied.
        /// Use this for endpoints that accept validated request objects.
        /// </summary>
        /// <typeparam name="TRequest">The type of request to validate.</typeparam>
        /// <param name="prefix">The route prefix for the group (e.g., "/api/v1").</param>
        /// <returns>The route group builder for further configuration.</returns>
        public RouteGroupBuilder MapApiGroup<TRequest>(string prefix)
            where TRequest : class
        {
            return builder
                .MapGroup(prefix)
                .AddEndpointFilter<ExceptionHandlingEndpointFilter>()
                .AddEndpointFilter<ValidationEndpointFilter<TRequest>>();
        }

        /// <summary>
        /// Creates a route group for file upload endpoints with standard filters.
        /// Includes exception handling, file upload validation, and image extension validation.
        /// </summary>
        /// <param name="prefix">The route prefix for the group (e.g., "/api/v1").</param>
        /// <param name="imageOnly">Whether to enforce image-only uploads. Default: true.</param>
        /// <returns>The route group builder for further configuration.</returns>
        public RouteGroupBuilder MapFileUploadGroup(string prefix, bool imageOnly = true)
        {
            var group = builder
                .MapGroup(prefix)
                .AddEndpointFilter<ExceptionHandlingEndpointFilter>()
                .AddEndpointFilter<FileUploadValidationFilter>();

            if (imageOnly)
            {
                group.AddEndpointFilter<ImageExtensionValidationFilter>();
            }

            return group;
        }

        /// <summary>
        /// Adds the standard exception handling filter to an endpoint.
        /// </summary>
        /// <param name="endpointBuilder">The route handler builder.</param>
        /// <returns>The route handler builder for chaining.</returns>
        public RouteHandlerBuilder WithExceptionHandling(RouteHandlerBuilder endpointBuilder)
        {
            return endpointBuilder.AddEndpointFilter<ExceptionHandlingEndpointFilter>();
        }

        /// <summary>
        /// Adds the standard validation filter to an endpoint.
        /// </summary>
        /// <typeparam name="TRequest">The type of request to validate.</typeparam>
        /// <param name="endpointBuilder">The route handler builder.</param>
        /// <returns>The route handler builder for chaining.</returns>
        public RouteHandlerBuilder WithValidation<TRequest>(RouteHandlerBuilder endpointBuilder)
            where TRequest : class
        {
            return endpointBuilder.AddEndpointFilter<ValidationEndpointFilter<TRequest>>();
        }

        /// <summary>
        /// Adds file upload validation filters to an endpoint.
        /// </summary>
        /// <param name="endpointBuilder">The route handler builder.</param>
        /// <param name="imageOnly">Whether to enforce image-only uploads. Default: true.</param>
        /// <returns>The route handler builder for chaining.</returns>
        public RouteHandlerBuilder WithFileUpload(
            RouteHandlerBuilder endpointBuilder,
            bool imageOnly = true
        )
        {
            endpointBuilder.AddEndpointFilter<FileUploadValidationFilter>();

            if (imageOnly)
            {
                endpointBuilder.AddEndpointFilter<ImageExtensionValidationFilter>();
            }

            return endpointBuilder;
        }

        /// <summary>
        /// Adds standard metadata tags to the endpoint for OpenAPI documentation.
        /// </summary>
        /// <param name="endpointBuilder">The route handler builder.</param>
        /// <param name="tag">The tag name for grouping in OpenAPI.</param>
        /// <param name="summary">A brief summary of the endpoint.</param>
        /// <param name="description">A detailed description of the endpoint.</param>
        /// <returns>The route handler builder for chaining.</returns>
        public RouteHandlerBuilder WithApiMetadata(
            RouteHandlerBuilder endpointBuilder,
            string tag,
            string? summary = null,
            string? description = null
        )
        {
            endpointBuilder.WithTags(tag);

            if (!string.IsNullOrEmpty(summary))
            {
                endpointBuilder.WithSummary(summary);
            }

            if (!string.IsNullOrEmpty(description))
            {
                endpointBuilder.WithDescription(description);
            }

            return endpointBuilder;
        }

        /// <summary>
        /// Configures the endpoint to require authorization.
        /// </summary>
        /// <param name="endpointBuilder">The route handler builder.</param>
        /// <param name="policy">Optional authorization policy name.</param>
        /// <returns>The route handler builder for chaining.</returns>
        public RouteHandlerBuilder WithAuthorization(
            RouteHandlerBuilder endpointBuilder,
            string? policy = null
        )
        {
            if (string.IsNullOrEmpty(policy))
            {
                endpointBuilder.RequireAuthorization();
            }
            else
            {
                endpointBuilder.RequireAuthorization(policy);
            }

            return endpointBuilder;
        }

        /// <summary>
        /// Configures the endpoint to allow anonymous access.
        /// </summary>
        /// <param name="endpointBuilder">The route handler builder.</param>
        /// <returns>The route handler builder for chaining.</returns>
        public RouteHandlerBuilder AllowAnonymous(RouteHandlerBuilder endpointBuilder)
        {
            return endpointBuilder.AllowAnonymous();
        }
    }
}
