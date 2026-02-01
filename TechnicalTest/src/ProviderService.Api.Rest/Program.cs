using ProviderService.Api.Rest.Middlewares;
using ProviderService.Application;
using ProviderService.Infrastructure;
using ProviderService.Presentation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPresentation();
builder.Services.AddTransient<GlobalErrorHandlingMiddleware>();

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.MapControllers();

app.Run();