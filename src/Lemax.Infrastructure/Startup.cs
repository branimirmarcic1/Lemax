using Lemax.Infrastructure.Common;
using Lemax.Infrastructure.Mapping;
using Lemax.Infrastructure.Middleware;
using Lemax.Infrastructure.Persistence;
using Lemax.Infrastructure.Persistence.Initialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure;

[ExcludeFromCodeCoverage]
public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        MapsterSettings.Configure();
        return services
            .AddExceptionMiddleware()
            .AddPersistence(config)
            .AddServices(config);
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
            .UseExceptionMiddleware();
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers();
        return builder;
    }
}