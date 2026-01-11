namespace Standards.Net.Application.Commands;

/// <summary>
/// Represents a void return value for commands that do not return data.
/// This is a singleton type used as a placeholder for commands with no response.
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
    /// <summary>
    /// The singleton instance of Unit.
    /// </summary>
    public static readonly Unit Value = default;

    /// <summary>
    /// Returns a completed task with the Unit value.
    /// Useful for async methods that need to return Task&lt;Unit&gt;.
    /// </summary>
    public static Task<Unit> Task => System.Threading.Tasks.Task.FromResult(Value);

    /// <inheritdoc />
    public bool Equals(Unit other) => true;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Unit;

    /// <inheritdoc />
    public override int GetHashCode() => 0;

    /// <inheritdoc />
    public override string ToString() => "()";

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(Unit left, Unit right) => true;

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(Unit left, Unit right) => false;
}
