using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Standards.Net.Application.Commands;
using Standards.Net.Application.Dispatchers;
using Standards.Net.Application.Events;
using Standards.Net.Application.Queries;

namespace Standards.Net.Application.Extensions;

/// <summary>
/// Extension methods for registering application layer services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the command/query dispatcher and domain event dispatcher in the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddApplicationDispatchers(this IServiceCollection services)
    {
        services.AddScoped<ICommandQueryDispatcher, CommandQueryDispatcher>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        return services;
    }

    /// <summary>
    /// Registers all command and query handlers from the specified assembly automatically.
    /// Uses reflection to discover and register all ICommandHandler and IQueryHandler implementations.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="applicationAssembly">The assembly containing handler implementations.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddApplicationHandlers(
        this IServiceCollection services,
        Assembly applicationAssembly
    )
    {
        // Register command handlers
        services.AddCommandHandlers(applicationAssembly);

        // Register query handlers
        services.AddQueryHandlers(applicationAssembly);

        // Register domain event handlers
        services.AddEventHandlers(applicationAssembly);

        return services;
    }

    /// <summary>
    /// Registers all command handlers from the specified assembly.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly containing command handler implementations.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddCommandHandlers(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        var commandHandlerTypes = assembly
            .GetTypes()
            .Where(t =>
                t.IsClass
                && !t.IsAbstract
                && t.GetInterfaces()
                    .Any(i =>
                        i.IsGenericType
                        && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                    )
            )
            .ToList();

        foreach (var handlerType in commandHandlerTypes)
        {
            var interfaceType = handlerType
                .GetInterfaces()
                .First(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                );

            services.AddScoped(interfaceType, handlerType);
        }

        return services;
    }

    /// <summary>
    /// Registers all query handlers from the specified assembly.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly containing query handler implementations.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddQueryHandlers(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        var queryHandlerTypes = assembly
            .GetTypes()
            .Where(t =>
                t.IsClass
                && !t.IsAbstract
                && t.GetInterfaces()
                    .Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)
                    )
            )
            .ToList();

        foreach (var handlerType in queryHandlerTypes)
        {
            var interfaceType = handlerType
                .GetInterfaces()
                .First(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)
                );

            services.AddScoped(interfaceType, handlerType);
        }

        return services;
    }

    /// <summary>
    /// Registers all domain event handlers from the specified assembly.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly containing event handler implementations.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddEventHandlers(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        var eventHandlerTypes = assembly
            .GetTypes()
            .Where(t =>
                t.IsClass
                && !t.IsAbstract
                && t.GetInterfaces()
                    .Any(i =>
                        i.IsGenericType
                        && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)
                    )
            )
            .ToList();

        foreach (var handlerType in eventHandlerTypes)
        {
            var interfaceType = handlerType
                .GetInterfaces()
                .First(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)
                );

            services.AddScoped(interfaceType, handlerType);
        }

        return services;
    }
}
