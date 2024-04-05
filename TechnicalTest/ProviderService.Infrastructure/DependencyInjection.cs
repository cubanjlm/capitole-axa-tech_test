using Microsoft.Extensions.DependencyInjection;
using ProviderService.Infrastructure.Repositories;
using ProvideService.Domain.Repositories;
using Raven.DependencyInjection;

namespace ProviderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddRavenDbDocStore().AddRavenDbAsyncSession();
        services.AddTransient<IWriteProviderRepository, WriteProviderRepository>();
        services.AddTransient<IFetchProviderRepository, FetchProviderRepository>();
        
        return services;
    } 
}