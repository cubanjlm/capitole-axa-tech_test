using ProvideService.Domain.Entities;

namespace ProvideService.Domain.Repositories;

public interface IWriteProviderRepository
{
    public Task<List<(int Id, string Name)>> ImportProviders(List<Provider> providers);
}