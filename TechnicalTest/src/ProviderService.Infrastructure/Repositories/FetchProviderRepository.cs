using ProvideService.Domain.Entities;
using ProvideService.Domain.Repositories;
using Raven.Client.Documents.Session;

namespace ProviderService.Infrastructure.Repositories;

public class FetchProviderRepository : IFetchProviderRepository
{
    private readonly IAsyncDocumentSession _dbSession;

    public FetchProviderRepository(IAsyncDocumentSession dbSession)
    {
        _dbSession = dbSession;
    }
    public async Task<Provider?> GetProviderById(int providerId)
    {
        var buildId = Provider.BuildId(providerId);
        var provider = await _dbSession.LoadAsync<Provider>(buildId);
        return provider;
    }
}