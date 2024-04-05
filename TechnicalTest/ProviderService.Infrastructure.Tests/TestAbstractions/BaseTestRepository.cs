using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.TestDriver;

namespace ProviderService.Infrastructure.Tests.TestAbstractions;

public abstract class BaseTestRepository : RavenTestDriver
{
    protected IDocumentStore Store = null!;
    private static bool _serverRunning;

    protected void StartTestServer(string? databaseName = null)
    {
        StartServerIfNotRunning();
        Store = GetDocumentStore(database: databaseName ?? Guid.NewGuid().ToString());
    }

    public override void Dispose()
    {
        base.Dispose();
        Store.Dispose();
    }

    private static void StartServerIfNotRunning()
    {
        if (_serverRunning) return;
        _serverRunning = true;
        ConfigureServer(new TestServerOptions
        {
            //DataDirectory = "./RavenDbTest/",
        });
    }

    protected async Task ArrangeData<T>(params T[] data)
    {
        BulkInsertOperation? bulkInsert = null;
        try
        {
            bulkInsert = Store.BulkInsert();
            foreach (var record in data)
            {
                await bulkInsert.StoreAsync(record);
            }
        }
        finally
        {
            if (bulkInsert != null)
            {
                await bulkInsert.DisposeAsync().ConfigureAwait(false);
            }

            WaitForIndexing(Store);
        }
    }
}