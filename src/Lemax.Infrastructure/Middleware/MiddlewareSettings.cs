using System.Diagnostics.CodeAnalysis;

namespace Lemax.Infrastructure.Middleware;

[ExcludeFromCodeCoverage]
public class MiddlewareSettings
{
    public bool EnableHttpsLogging { get; set; } = false;
}