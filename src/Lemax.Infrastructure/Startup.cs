using Lemax.Infrastructure.Common;
using Lemax.Infrastructure.Mapping;
using Lemax.Infrastructure.Middleware;
using Lemax.Infrastructure.Persistence;
using Lemax.Infrastructure.Persistence.Initialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;

namespace Lemax.Infrastructure;

[ExcludeFromCodeCoverage]
public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        MapsterSettings.Configure();

        services
            .AddExceptionMiddleware()
            .AddPersistence(config)
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices(config);

        services
            .AddHealthChecks();

        return services;
    }

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using IServiceScope? scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config)
    {
        return builder
            .UseExceptionMiddleware()
            .UseRouting()
            .UseRequestLogging(config);
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers();

        builder.MapGet("/health", async (HealthCheckService healthService) =>
        {
            var report = await healthService.CheckHealthAsync();
            return report.Status == HealthStatus.Healthy
                ? Results.Ok(new { Status = report.Status.ToString(), Details = report.Entries })
                : Results.StatusCode(503);
        })
        .WithTags("System Health")
        .WithName("CheckHealth")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Application Health Status";
            operation.Description = "Provjerava sustav i bazu podataka (SQL ili In-Memory).";
            return operation;
        });

        return builder;
    }
}