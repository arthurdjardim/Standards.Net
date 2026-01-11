namespace Standards.Net.API.Services;

/// <summary>
/// Thread-safe implementation of <see cref="IApiRequestContext"/> using AsyncLocal storage.
/// Provides request-scoped context that flows correctly across async/await boundaries.
/// </summary>
public sealed class ApiRequestContext : IApiRequestContext
{
    private static readonly AsyncLocal<RequestContextData> _context = new();

    private RequestContextData Context
    {
        get => _context.Value ??= new RequestContextData();
        set => _context.Value = value;
    }

    public string? TenantId => Context.TenantId;
    public string? UserId => Context.UserId;
    public string CorrelationId => Context.CorrelationId ??= Guid.NewGuid().ToString();
    public string? IpAddress => Context.IpAddress;
    public string? UserAgent => Context.UserAgent;

    public void SetTenantId(string? tenantId) => Context.TenantId = tenantId;

    public void SetUserId(string? userId) => Context.UserId = userId;

    public void SetCorrelationId(string correlationId) => Context.CorrelationId = correlationId;

    public void SetIpAddress(string? ipAddress) => Context.IpAddress = ipAddress;

    public void SetUserAgent(string? userAgent) => Context.UserAgent = userAgent;

    public T? GetValue<T>(string key)
    {
        if (Context.CustomValues.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }

    public void SetValue<T>(string key, T value)
    {
        if (value is not null)
        {
            Context.CustomValues[key] = value;
        }
        else
        {
            Context.CustomValues.Remove(key);
        }
    }

    public void Clear()
    {
        _context.Value = null!;
    }

    private sealed class RequestContextData
    {
        public string? TenantId { get; set; }
        public string? UserId { get; set; }
        public string? CorrelationId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public Dictionary<string, object> CustomValues { get; } = new();
    }
}
