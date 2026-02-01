using ProvideService.Domain.Entities;

namespace ProvideService.Domain.Repositories;

/// <summary>
/// to show layer decoupling on for database fetching flow 
/// </summary>
public interface IFetchProviderRepository
{
    public Task<Provider?> GetProviderById(int providerId);
}