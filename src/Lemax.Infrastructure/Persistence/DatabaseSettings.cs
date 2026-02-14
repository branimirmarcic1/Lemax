using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class DatabaseSettings
{
    public string? DBProvider { get; set; }
    public string? ConnectionString { get; set; }
}