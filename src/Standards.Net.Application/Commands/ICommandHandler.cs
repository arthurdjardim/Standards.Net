namespace Standards.Net.Application.Commands;

/// <summary>
/// Interface for command handlers that process commands and execute business logic.
/// Each command should have exactly one handler implementation.
/// </summary>
/// <typeparam name="TCommand">The type of command this handler processes.</typeparam>
/// <typeparam name="TResponse">The type of response returned after command execution.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    /// <summary>
    /// Handles the execution of a command asynchronously.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response result after command execution.</returns>
    Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

/// <summary>
/// Interface for command handlers that do not return a value.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle.</typeparam>
public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand { }
