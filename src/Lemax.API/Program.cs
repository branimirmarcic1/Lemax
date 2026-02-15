using FluentValidation.AspNetCore;
using Lemax.API.Configurations;
using Lemax.Application;
using Lemax.Infrastructure;
using Lemax.Infrastructure.Common;
using Microsoft.OpenApi.Models;
using NSwag;
using Serilog;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Host.AddConfigurations();
    builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration);
    });

    builder.Services.AddControllers();
    builder.Services.AddFluentValidationAutoValidation();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Lemax.API", Version = "1.0" });
        options.DocInclusionPredicate((docName, apiDesc) =>
        {
            var path = apiDesc.RelativePath?.ToLower() ?? "";
            // Sakrij sve identity rute OSIM register i login
            if (path.Contains("manage") || path.Contains("refresh") || path.Contains("confirm") || path.Contains("forgot") || path.Contains("reset"))
            {
                return false;
            }
            return true;
        });
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type =  SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In =  ParameterLocation.Header,
            Description = "Unesite samo vaš token (bez 'Bearer' riječi)."
        });

        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new  Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    });

    WebApplication? app = builder.Build();
    await app.Services.InitializeDatabasesAsync();
    app.UseInfrastructure(builder.Configuration);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapEndpoints();

    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}

