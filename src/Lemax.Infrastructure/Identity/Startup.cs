using Lemax.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure.Identity;

internal static class Startup
{
    internal static IServiceCollection AddIdentityAuth(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<LemaxDbContext>();

        return services;
    }
}