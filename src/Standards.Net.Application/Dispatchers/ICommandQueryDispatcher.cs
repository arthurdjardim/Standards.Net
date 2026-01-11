using Standards.Net.Application.Commands;
using Standards.Net.Application.Queries;

namespace Standards.Net.Application.Dispatchers;

/// <summary>
/// Central dispatcher for commands and queries.
/// Provides a unified interface for executing commands and queries with automatic validation.
/// This is a custom implementation replacing MediatR for licensing considerations.
/// </summary>
public interface ICommandQueryDispatcher
{
    /// <summary>
    /// Dispatches a command to its registered handler and executes it.
    /// Automatically validates the command before execution using FluentValidation if validators are registered.
    /// </summary>
    /// <typeparam name="TResponse">The type of response returned by the command.</typeparam>
    /// <param name="command">The command to dispatch and execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the command execution.</returns>
    Task<TResponse> DispatchCommandAsync<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Dispatches a query to its registered handler and executes it.
    /// </summary>
    /// <typeparam name="TResponse">The type of data returned by the query.</typeparam>
    /// <param name="query">The query to dispatch and execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The data result of the query execution.</returns>
    Task<TResponse> DispatchQueryAsync<TResponse>(
        IQuery<TResponse> query,
        CancellationToken cancellationToken = default
    );
}
