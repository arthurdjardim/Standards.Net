using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Standards.Net.API.Filters;
using Standards.Net.API.Middleware;
using Standards.Net.API.Options;
using Standards.Net.API.Services;

namespace Standards.Net.API.Extensions;

/// <summary>
/// Extension methods for configuring services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds all Default API Standards services with the provided configuration.
        /// This is the main entry point for configuring the library.
        /// </summary>
        /// <param name="configuration">The configuration section containing ApiStandardsOptions.</param>
        /// <param name="configureOptions">Optional action to configure options programmatically.</param>
        /// <returns>The service collection for chaining.</returns>
        public IServiceCollection AddDefaultApi(
            IConfiguration configuration,
            Action<ApiStandardsOptions>? configureOptions = null
        )
        {
            // Bind and configure options
            var options = new ApiStandardsOptions();
            configuration.Bind(options);
            configureOptions?.Invoke(options);

            services.Configure<ApiStandardsOptions>(opt =>
            {
                configuration.Bind(opt);
                configureOptions?.Invoke(opt);
            });
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.Configure<CorsOptions>(configuration.GetSection("Cors"));
            services.Configure<OpenApiOptions>(configuration.GetSection("OpenApi"));
            services.Configure<FileUploadOptions>(configuration.GetSection("FileUpload"));

            // Register core services
            services.AddSingleton<IApiRequestContext, ApiRequestContext>();

            // Add endpoint filters
            services.AddScoped<ExceptionHandlingEndpointFilter>();
            services.AddScoped(typeof(ValidationEndpointFilter<>));
            services.AddScoped<FileUploadValidationFilter>();
            services.AddScoped<ImageExtensionValidationFilter>();

            // Add OpenAPI if enabled
            if (options.EnableOpenApi && options.OpenApi.Enabled)
            {
                services.AddOpenApiServices(options.OpenApi);
            }

            // Add CORS if enabled
            if (options.EnableCors)
            {
                services.AddCorsServices(options.Cors);
            }

            // Add response compression if enabled
            if (options.EnableResponseCompression)
            {
                // TODO: Requires Microsoft.AspNetCore.ResponseCompression package
                // services.AddResponseCompression(compressionOptions =>
                // {
                //     compressionOptions.EnableForHttps = true;
                // });
            }

            // Add health checks if enabled
            if (options.EnableHealthChecks)
            {
                services.AddHealthChecks();
            }

            // Add rate limiting if enabled
            if (options.EnableRateLimiting)
            {
                // TODO: Rate limiting is built-in for .NET 10, verify usage
                // services.AddRateLimiter(rateLimitOptions =>
                // {
                //     rateLimitOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                // });
            }

            return services;
        }

        /// <summary>
        /// Adds JWT Bearer authentication with the provided configuration.
        /// </summary>
        /// <param name="configuration">The configuration section containing JwtOptions.</param>
        /// <param name="configureOptions">Optional action to configure JWT options programmatically.</param>
        /// <returns>The service collection for chaining.</returns>
        public IServiceCollection AddDefaultJwtAuthentication(
            IConfiguration configuration,
            Action<JwtOptions>? configureOptions = null
        )
        {
            var jwtOptions = new JwtOptions();
            configuration.Bind(jwtOptions);
            configureOptions?.Invoke(jwtOptions);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = jwtOptions.ValidateIssuer,
                        ValidateAudience = jwtOptions.ValidateAudience,
                        ValidateLifetime = jwtOptions.ValidateLifetime,
                        ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.SecretKey)
                        ),
                        ClockSkew = jwtOptions.ClockSkew,
                    };
                });

            return services;
        }

        /// <summary>
        /// Adds standard authorization policies for common scenarios.
        /// </summary>
        /// <param name="configurePolicies">Action to configure custom authorization policies.</param>
        /// <returns>The service collection for chaining.</returns>
        public IServiceCollection AddDefaultAuthorization(
            Action<Microsoft.AspNetCore.Authorization.AuthorizationOptions>? configurePolicies =
                null
        )
        {
            services.AddAuthorization(options =>
            {
                configurePolicies?.Invoke(options);
            });

            return services;
        }

        /// <summary>
        /// Adds OpenAPI documentation services.
        /// </summary>
        private IServiceCollection AddOpenApiServices(OpenApiOptions options)
        {
            services.AddOpenApi();

            if (options.UseScalar)
            {
                // Scalar is added via middleware, just ensure the package is referenced
            }

            return services;
        }

        /// <summary>
        /// Adds CORS services with the provided configuration.
        /// </summary>
        private IServiceCollection AddCorsServices(CorsOptions options)
        {
            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy(
                    options.PolicyName,
                    builder =>
                    {
                        // Configure origins
                        if (options.AllowAnyOrigin)
                        {
                            builder.AllowAnyOrigin();
                        }
                        else if (options.AllowedOrigins.Count > 0)
                        {
                            builder.WithOrigins(options.AllowedOrigins.ToArray());
                        }

                        // Configure methods
                        if (options.AllowAnyMethod)
                        {
                            builder.AllowAnyMethod();
                        }
                        else if (options.AllowedMethods.Count > 0)
                        {
                            builder.WithMethods(options.AllowedMethods.ToArray());
                        }

                        // Configure headers
                        if (options.AllowAnyHeader)
                        {
                            builder.AllowAnyHeader();
                        }
                        else if (options.AllowedHeaders.Count > 0)
                        {
                            builder.WithHeaders(options.AllowedHeaders.ToArray());
                        }

                        // Configure exposed headers
                        if (options.ExposedHeaders.Count > 0)
                        {
                            builder.WithExposedHeaders(options.ExposedHeaders.ToArray());
                        }

                        // Configure credentials
                        if (options.AllowCredentials && !options.AllowAnyOrigin)
                        {
                            builder.AllowCredentials();
                        }

                        // Configure preflight cache
                        if (options.PreflightMaxAge > 0)
                        {
                            builder.SetPreflightMaxAge(
                                TimeSpan.FromSeconds(options.PreflightMaxAge)
                            );
                        }
                    }
                );
            });

            return services;
        }
    }
}
