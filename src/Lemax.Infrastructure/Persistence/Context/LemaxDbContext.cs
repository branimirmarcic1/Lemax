using Lemax.Domain;
using Lemax.Infrastructure.Persistence.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure.Persistence.Context;

public class LemaxDbContext : IdentityDbContext<IdentityUser>
{
    public LemaxDbContext(DbContextOptions<LemaxDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hotel> Hotels => Set<Hotel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(HotelConfig).Assembly);
    }
}