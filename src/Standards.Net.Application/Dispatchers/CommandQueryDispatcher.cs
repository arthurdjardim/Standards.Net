using Microsoft.Extensions.DependencyInjection;
using Standards.Net.Application.Commands;
using Standards.Net.Application.Queries;

namespace Standards.Net.Application.Dispatchers;

/// <summary>
/// Central dispatcher for commands and queries following the CQRS pattern.
/// </summary>
/// <remarks>
/// <para>
/// This is a lightweight custom implementation that replaces MediatR for licensing considerations.
/// It resolves handlers from the dependency injection container and executes them via reflection.
/// </para>
/// <para>
/// <strong>Validation Strategy:</strong> Input validation is performed at the API layer using
/// validation filters with FluentValidation. Commands and queries arriving at this
/// dispatcher are assumed to be already validated.
/// </para>
/// <para>
/// <strong>Thread Safety:</strong> This dispatcher is registered as scoped in DI and is thread-safe
/// within the scope of a single HTTP request.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Dispatching a command
/// var command = new CreateUserCommand { Name = "John", Email = "john@example.com" };
/// var response = await dispatcher.DispatchCommandAsync&lt;CreateUserResponse&gt;(command);
///
/// // Dispatching a query
/// var query = new GetUserByIdQuery(userId);
/// var user = await dispatcher.DispatchQueryAsync&lt;UserDto&gt;(query);
/// </code>
/// </example>
public sealed class CommandQueryDispatcher(IServiceProvider serviceProvider)
    : ICommandQueryDispatcher
{
    /// <summary>
    /// Dispatches a command to its registered handler for execution.
    /// </summary>
    /// <typeparam name="TResponse">The type of response returned by the command handler.</typeparam>
    /// <param name="command">The command instance to dispatch.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the command execution result.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no handler is registered for the command type.
    /// </exception>
    public async Task<TResponse> DispatchCommandAsync<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken = default
    )
    {
        var handler = ResolveHandler<ICommand<TResponse>, TResponse>(
            command,
            typeof(ICommandHandler<,>)
        );
        return await InvokeHandlerAsync<TResponse>(
            handler,
            command,
            cancellationToken,
            nameof(ICommandHandler<ICommand<object>, object>.HandleAsync)
        );
    }

    /// <summary>
    /// Dispatches a query to its registered handler for data retrieval.
    /// </summary>
    /// <typeparam name="TResponse">The type of data returned by the query handler.</typeparam>
    /// <param name="query">The query instance to dispatch.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the query result data.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no handler is registered for the query type.
    /// </exception>
    public async Task<TResponse> DispatchQueryAsync<TResponse>(
        IQuery<TResponse> query,
        CancellationToken cancellationToken = default
    )
    {
        var handler = ResolveHandler<IQuery<TResponse>, TResponse>(query, typeof(IQueryHandler<,>));
        return await InvokeHandlerAsync<TResponse>(
            handler,
            query,
            cancellationToken,
            nameof(IQueryHandler<IQuery<object>, object>.HandleAsync)
        );
    }

    /// <summary>
    /// Resolves a handler from the DI container for the given request type.
    /// </summary>
    private object ResolveHandler<TRequest, TResponse>(
        TRequest request,
        Type handlerOpenGenericType
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var handlerType = handlerOpenGenericType.MakeGenericType(requestType, typeof(TResponse));

        var handler =
            serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException(
                $"No handler registered for {requestType.Name}. "
                    + $"Ensure a handler implementing {handlerType.Name} is registered in the DI container."
            );

        return handler;
    }

    /// <summary>
    /// Invokes the handler method via reflection.
    /// </summary>
    private static async Task<TResponse> InvokeHandlerAsync<TResponse>(
        object handler,
        object request,
        CancellationToken cancellationToken,
        string methodName
    )
    {
        var handleMethod =
            handler.GetType().GetMethod(methodName)
            ?? throw new InvalidOperationException(
                $"Handler {handler.GetType().Name} does not have a {methodName} method."
            );

        var resultTask =
            handleMethod.Invoke(handler, [request, cancellationToken]) as Task<TResponse>
            ?? throw new InvalidOperationException(
                $"Handler method did not return Task<{typeof(TResponse).Name}>."
            );

        return await resultTask;
    }
}
