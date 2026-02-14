using Lemax.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure.Persistence.Initialization;

internal class DatabaseInitializer : IDatabaseInitializer
{
    private readonly LemaxDbContext _lemaxDbContext;
    private readonly LemaxDbSeeder _dbSeeder;

    public DatabaseInitializer(LemaxDbContext lemaxDbContex, LemaxDbSeeder dbSeeder)
    {
        _lemaxDbContext = lemaxDbContex;
        _dbSeeder = dbSeeder;
    }

    public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
    {
        if (_lemaxDbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
        {
            await _lemaxDbContext.Database.EnsureCreatedAsync(cancellationToken);
            await _dbSeeder.SeedDatabaseAsync(_lemaxDbContext);
            return;
        }

        if (_lemaxDbContext.Database.GetMigrations().Any())
        {
            if ((await _lemaxDbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                await _lemaxDbContext.Database.MigrateAsync(cancellationToken);
            }

            if (await _lemaxDbContext.Database.CanConnectAsync(cancellationToken))
            {
                await _dbSeeder.SeedDatabaseAsync(_lemaxDbContext);
            }
        }
    }
}