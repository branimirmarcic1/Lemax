using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Application;

[ExcludeFromCodeCoverage]
public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        Assembly? assembly = Assembly.GetExecutingAssembly();
        return services
            .AddValidatorsFromAssembly(assembly);
    }
}
