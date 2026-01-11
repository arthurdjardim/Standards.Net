using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Standards.Net.API.Middleware;
using Standards.Net.API.Options;

namespace Standards.Net.API.Extensions;

/// <summary>
/// Extension methods for configuring the HTTP request pipeline.
/// </summary>
public static class ApplicationBuilderExtensions
{
    extension(IApplicationBuilder app)
    {
        /// <summary>
        /// Configures the complete Default API Standards middleware pipeline in the correct order.
        /// This includes request context, authentication, authorization, CORS, security headers, etc.
        /// </summary>
        /// <returns>The application builder for chaining.</returns>
        public IApplicationBuilder UseDefaultApiPipeline()
        {
            var options = app
                .ApplicationServices.GetRequiredService<IOptions<ApiStandardsOptions>>()
                .Value;
            var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();

            // 1. Response compression (must be early)
            if (options.EnableResponseCompression)
            {
                app.UseResponseCompression();
            }

            // 2. Security headers
            if (options.EnableSecurityHeaders)
            {
                app.UseMiddleware<SecurityHeadersMiddleware>();
            }

            // 3. Request logging (if enabled)
            if (options.EnableRequestLogging)
            {
                app.UseMiddleware<RequestLoggingMiddleware>();
            }

            // 4. HTTPS redirection (recommended for production)
            app.UseHttpsRedirection();

            // 5. CORS (before authentication)
            if (options.EnableCors)
            {
                app.UseCors(options.Cors.PolicyName);
            }

            // 6. Request context (before authentication to capture all requests)
            if (options.EnableRequestContext)
            {
                app.UseMiddleware<RequestContextMiddleware>();
            }

            // 7. Authentication
            app.UseAuthentication();

            // 8. Tenant validation (after authentication, before authorization)
            if (options.EnableMultiTenancy && options.ValidateTenantFromJwt)
            {
                app.UseMiddleware<TenantJwtValidationMiddleware>();
            }

            // 9. Authorization
            app.UseAuthorization();

            // 10. Rate limiting (if enabled)
            if (options.EnableRateLimiting)
            {
                app.UseRateLimiter();
            }

            // 11. OpenAPI/Scalar (only in development or if explicitly enabled)
            if (options.EnableOpenApi && options.OpenApi.Enabled && app is WebApplication webApp)
            {
                webApp.MapOpenApi();

                if (options.OpenApi.UseScalar)
                {
                    webApp.MapScalarApiReference(opt =>
                    {
                        opt.Title = options.OpenApi.Title;
                        opt.Theme = options.OpenApi.ScalarTheme;
                    });
                }
            }

            // 12. Health checks (if enabled)
            if (options.EnableHealthChecks)
            {
                // Health check endpoints will be mapped separately
            }

            return app;
        }

        /// <summary>
        /// Maps health check endpoints with standard paths.
        /// </summary>
        /// <param name="healthPath">The path for the detailed health check. Default: "/health".</param>
        /// <param name="livenessPath">The path for the liveness probe. Default: "/health/live".</param>
        /// <param name="readinessPath">The path for the readiness probe. Default: "/health/ready".</param>
        /// <returns>The application builder for chaining.</returns>
        public IApplicationBuilder MapDefaultHealthChecks(
            string healthPath = "/health",
            string livenessPath = "/health/live",
            string readinessPath = "/health/ready"
        )
        {
            var options = app
                .ApplicationServices.GetRequiredService<IOptions<ApiStandardsOptions>>()
                .Value;

            if (!options.EnableHealthChecks)
            {
                return app;
            }

            if (app is WebApplication webApp)
            {
                // Detailed health check
                webApp.MapHealthChecks(healthPath);

                // Liveness probe (basic check)
                webApp.MapHealthChecks(
                    livenessPath,
                    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                    {
                        Predicate = _ => false, // No checks, just returns healthy if app is running
                    }
                );

                // Readiness probe (all checks)
                webApp.MapHealthChecks(readinessPath);
            }

            return app;
        }

        /// <summary>
        /// Ensures the middleware pipeline uses request context tracking.
        /// </summary>
        /// <returns>The application builder for chaining.</returns>
        public IApplicationBuilder UseDefaultRequestContext()
        {
            return app.UseMiddleware<RequestContextMiddleware>();
        }

        /// <summary>
        /// Ensures the middleware pipeline uses tenant JWT validation.
        /// </summary>
        /// <returns>The application builder for chaining.</returns>
        public IApplicationBuilder UseDefaultTenantValidation()
        {
            return app.UseMiddleware<TenantJwtValidationMiddleware>();
        }

        /// <summary>
        /// Ensures the middleware pipeline uses security headers.
        /// </summary>
        /// <returns>The application builder for chaining.</returns>
        public IApplicationBuilder UseDefaultSecurityHeaders()
        {
            return app.UseMiddleware<SecurityHeadersMiddleware>();
        }

        /// <summary>
        /// Ensures the middleware pipeline uses request logging.
        /// </summary>
        /// <returns>The application builder for chaining.</returns>
        public IApplicationBuilder UseDefaultRequestLogging()
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
