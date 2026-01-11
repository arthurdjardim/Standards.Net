namespace Standards.Net.Application.Events;

/// <summary>
/// Interface for dispatching domain events to their respective handlers.
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches a collection of domain events to all registered handlers.
    /// Each event may have multiple handlers that will be invoked sequentially.
    /// </summary>
    /// <param name="events">The collection of domain events to dispatch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DispatchEventsAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default
    );
}
