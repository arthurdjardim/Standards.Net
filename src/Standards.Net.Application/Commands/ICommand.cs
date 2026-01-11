namespace Standards.Net.Application.Commands;

/// <summary>
/// Marker interface for all commands in the application.
/// Commands represent write operations that change the state of the system.
/// </summary>
/// <typeparam name="TResponse">The type of response returned after command execution.</typeparam>
public interface ICommand<TResponse> { }

/// <summary>
/// Marker interface for commands that do not return a value.
/// Use this for commands that only perform side effects.
/// </summary>
public interface ICommand : ICommand<Unit> { }
