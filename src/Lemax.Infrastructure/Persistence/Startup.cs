using Lemax.Infrastructure.Persistence.Context;
using Lemax.Infrastructure.Persistence.Initialization;
using Lemax.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure.Persistence;

internal static class Startup
{
    internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        List<DatabaseSettings>? databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<List<DatabaseSettings>>();

        DatabaseSettings sqlDbSettings = databaseSettings.First(x => x.DBProvider.Equals(Databases.InMemory));

        return services
            .Configure<DatabaseSettings>(config.GetSection(nameof(DatabaseSettings)))
                .AddDbContext<LemaxDbContext>(m => m.UseDatabase(sqlDbSettings.DBProvider, sqlDbSettings.ConnectionString),
                contextLifetime: ServiceLifetime.Transient,
                optionsLifetime: ServiceLifetime.Singleton)
                .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
                .AddTransient<LemaxDbSeeder>();
    }

    internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
    {
        switch (dbProvider.ToLowerInvariant())
        {
            case Databases.SQL:
                return builder.UseSqlServer(connectionString
                    , e => e.MigrationsAssembly("Lemax.SQL"));

            case Databases.InMemory:
                return builder.UseInMemoryDatabase("LemaxDb");

            default:
                throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
        }
    }
}