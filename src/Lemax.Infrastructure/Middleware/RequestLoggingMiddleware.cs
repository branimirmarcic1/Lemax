using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Lemax.Infrastructure.Middleware;

[ExcludeFromCodeCoverage]
public class RequestLoggingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        LogContext.PushProperty("RequestTimeUTC", DateTime.UtcNow);
        string requestBody = string.Empty;

        HttpRequest request = httpContext.Request;

        if (!string.IsNullOrEmpty(request.ContentType)
            && request.ContentType.StartsWith("application/json"))
        {
            request.EnableBuffering();
            using StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 4096, true);
            requestBody = await reader.ReadToEndAsync();

            request.Body.Position = 0;
        }

        LogContext.PushProperty("RequestBody", requestBody);
        Log.ForContext("RequestHeaders", httpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
           .ForContext("RequestBody", requestBody)
           .Information("HTTP {RequestMethod} Request sent to {RequestPath}", httpContext.Request.Method, httpContext.Request.Path);
        await next(httpContext);
    }
}
