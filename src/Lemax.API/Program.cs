using FluentValidation.AspNetCore;
using Lemax.API.Configurations;
using Lemax.Application.Hotels;
using Lemax.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddConfigurations();
// Add services to the container.
builder.Services
    .AddControllers()
    .AddFluentValidation(fv =>
    fv.RegisterValidatorsFromAssemblyContaining<CreateHotelRequestValidator>());

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.Services.InitializeDatabasesAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
