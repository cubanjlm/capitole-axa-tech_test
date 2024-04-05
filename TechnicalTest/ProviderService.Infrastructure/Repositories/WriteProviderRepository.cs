using ProvideService.Domain.Entities;
using ProvideService.Domain.Repositories;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;

namespace ProviderService.Infrastructure.Repositories;

public class WriteProviderRepository : IWriteProviderRepository
{
    private readonly IDocumentStore _store;

    public WriteProviderRepository(IDocumentStore store)
    {
        _store = store;
    }

    public async Task<List<(int Id, string Name)>> ImportProviders(List<Provider> providers)
    {
        var migrationResult = new List<(int Id, string Name)>();
        
        using var bulkInsert = _store.BulkInsert();
        foreach (var provider in providers)
        {
            await bulkInsert.StoreAsync(provider);
            migrationResult.Add((provider.CleanId(), provider.Name));
        }

        return migrationResult;
        
    }
}