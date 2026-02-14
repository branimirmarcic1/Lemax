using Lemax.Infrastructure.Common;
using Lemax.Infrastructure.Mapping;
using Lemax.Infrastructure.Persistence;
using Lemax.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        MapsterSettings.Configure();
        return services
            .AddPersistence(config)
            .AddServices(config);
    }

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using IServiceScope? scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }
}