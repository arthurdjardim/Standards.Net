namespace Standards.Net.Application.Events;

/// <summary>
/// Interface for domain event handlers.
/// Handlers react to domain events and perform side effects (sending emails, updating read models, etc.).
/// Multiple handlers can subscribe to the same event.
/// </summary>
/// <typeparam name="TEvent">The type of domain event this handler processes.</typeparam>
public interface IDomainEventHandler<in TEvent>
    where TEvent : IDomainEvent
{
    /// <summary>
    /// Handles the domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}
