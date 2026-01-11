namespace Standards.Net.Application.Queries;

/// <summary>
/// Marker interface for all queries in the application.
/// Queries represent read operations that do not change the state of the system.
/// </summary>
/// <typeparam name="TResponse">The type of data returned by the query.</typeparam>
public interface IQuery<TResponse> { }
