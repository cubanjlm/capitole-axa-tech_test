using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProviderService.Presentation.Errors;

namespace ProviderService.Api.Rest.Middlewares;

public class GlobalErrorHandlingMiddleware:IMiddleware
{
    private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

    public GlobalErrorHandlingMiddleware(ILogger<GlobalErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            var genericErrorHeader = "An error server occured.";
            
            _logger.LogError(e, genericErrorHeader);
            
            var problem = new CustomProblem("InternalServerError", genericErrorHeader);
            var json = JsonSerializer.Serialize(problem);
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(json);
        }
    }
}