namespace Standards.Net.Application.Events;

/// <summary>
/// Marker interface for domain events.
/// Domain events represent something that happened in the domain that domain experts care about.
/// Events are dispatched after the entity changes are persisted to the database.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the UTC date and time when this event occurred.
    /// </summary>
    DateTime OccurredAt { get; }
}
