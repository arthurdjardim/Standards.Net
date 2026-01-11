namespace Standards.Net.Application.Queries;

/// <summary>
/// Interface for query handlers that process queries and retrieve data.
/// Each query should have exactly one handler implementation.
/// </summary>
/// <typeparam name="TQuery">The type of query this handler processes.</typeparam>
/// <typeparam name="TResponse">The type of data returned by the query.</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    /// <summary>
    /// Handles the execution of a query asynchronously.
    /// </summary>
    /// <param name="query">The query to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The query result data.</returns>
    Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
