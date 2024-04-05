using ProviderService.Infrastructure.Repositories;
using ProviderService.Infrastructure.Tests.TestAbstractions;
using ProvideService.Domain.Entities;
using ProvideService.Domain.Enums;
using ProvideService.Domain.ValueObjects;
using Shouldly;

namespace ProviderService.Infrastructure.Tests.Repositories;

public class WriteProviderRepositoryTests : BaseTestRepository
{
    public WriteProviderRepositoryTests()
    {
        StartTestServer();
    }

    [Fact]
    public async Task GivenASetOfProviders_WhenImportingData_ThenTheFullListOfProvidersGetsStored()
    {
        //Arrange
        var providers = new List<Provider>
        {
            new(1, "Test Provider 1",
                PostalAddress.Create("Middle of nowhere, Betera"), ProviderType.rental),
            new(2, "Test Provider 2",
                PostalAddress.Create("Downtown, Valencia"), ProviderType.vfh)
        };

        var expectedMigrationResult = new List<(int Id, string Name)>
        {
            (1, "Test Provider 1"),
            (2, "Test Provider 2")
        };
        var expectedStoredIds = new[] { "providers/1", "providers/2" };

        var repository = new WriteProviderRepository(Store);

        // Act
        var result = await repository.ImportProviders(providers);
        WaitForUserToContinueTheTest(Store);

        // // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(expectedMigrationResult);
        using var arrangeSession = Store.OpenAsyncSession();
        var storedProviders = await arrangeSession.LoadAsync<Provider>(expectedStoredIds);
        storedProviders[expectedStoredIds[0]].Name.ShouldBe("Test Provider 1");
        storedProviders[expectedStoredIds[1]].Name.ShouldBe("Test Provider 2");
        storedProviders.Count.ShouldBe(result.Count);
    }

    [Fact]
    public async Task ImportProviders_NullProviders_ReturnsEmptyList()
    {
        // Arrange
        var repository = new WriteProviderRepository(Store);

        // Act
        var result = await repository.ImportProviders(new List<Provider>());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }
}