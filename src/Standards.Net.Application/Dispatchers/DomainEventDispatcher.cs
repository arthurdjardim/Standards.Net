using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Standards.Net.Application.Events;

namespace Standards.Net.Application.Dispatchers;

/// <summary>
/// Implementation of domain event dispatcher.
/// Uses reflection to resolve and invoke domain event handlers from the DI container.
/// Supports multiple handlers per event type.
/// </summary>
public sealed class DomainEventDispatcher(
    IServiceProvider serviceProvider,
    ILogger<DomainEventDispatcher> logger
) : IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches a collection of domain events to all registered handlers.
    /// </summary>
    /// <param name="events">The collection of domain events to dispatch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task DispatchEventsAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default
    )
    {
        var eventsList = events.ToList();

        if (eventsList.Count == 0)
        {
            return;
        }

        logger.LogInformation("Dispatching {EventCount} domain events", eventsList.Count);

        foreach (var domainEvent in eventsList)
        {
            var eventType = domainEvent.GetType();
            logger.LogDebug("Dispatching event: {EventType}", eventType.Name);

            // Get all handlers for this event type
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
            var handlersEnumerable = serviceProvider.GetServices(handlerType);
            var handlers = handlersEnumerable.ToList();

            if (handlers.Count == 0)
            {
                logger.LogWarning("No handlers registered for event: {EventType}", eventType.Name);
                continue;
            }

            logger.LogDebug(
                "Found {HandlerCount} handler(s) for event: {EventType}",
                handlers.Count,
                eventType.Name
            );

            // Invoke each handler
            foreach (var handler in handlers)
            {
                if (handler is null)
                {
                    logger.LogWarning(
                        "Null handler instance found for event: {EventType}",
                        eventType.Name
                    );
                    continue;
                }

                try
                {
                    var handleMethod = handlerType.GetMethod(
                        nameof(IDomainEventHandler<IDomainEvent>.HandleAsync)
                    );
                    if (handleMethod is not null)
                    {
                        var task = (Task?)
                            handleMethod.Invoke(handler, [domainEvent, cancellationToken]);
                        if (task is not null)
                        {
                            await task;
                        }
                    }

                    logger.LogDebug(
                        "Successfully invoked handler {HandlerType} for event {EventType}",
                        handler.GetType().Name,
                        eventType.Name
                    );
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "Error invoking handler {HandlerType} for event {EventType}",
                        handler.GetType().Name,
                        eventType.Name
                    );
                }
            }
        }

        logger.LogInformation("Completed dispatching {EventCount} domain events", eventsList.Count);
    }
}
