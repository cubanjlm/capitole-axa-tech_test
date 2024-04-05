using ProviderService.Infrastructure.Repositories;
using ProviderService.Infrastructure.Tests.TestAbstractions;
using ProvideService.Domain.Entities;
using ProvideService.Domain.Enums;
using ProvideService.Domain.ValueObjects;
using Shouldly;

namespace ProviderService.Infrastructure.Tests.Repositories;

public class FetchProviderRepositoryTests : BaseTestRepository
{
    public FetchProviderRepositoryTests()
    {
        StartTestServer();
    }

    [Fact]
    public async Task GivenAProviderById_WhenRequestingDataById_ThenAValidProviderIsReturned()
    {
        // Arrange
        var providerId = 123;
        var expectedProvider = new Provider(providerId, "Test Provider",
            PostalAddress.Create("Middle of nowhere, Betera"), ProviderType.rental)
        {
            CreatedAt = DateTimeOffset.UtcNow
        };
        await ArrangeData(expectedProvider);
        
        using var dbSession = Store.OpenAsyncSession();
        var repository = new FetchProviderRepository(dbSession);

        // Act
        var result = await repository.GetProviderById(providerId);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(expectedProvider);
    }

    [Fact]
    public async Task GivenAProviderById_WhenRequestingDataById_ThenANullValueIsReturned()
    {
        // Arrange
        using var dbSession = Store.OpenAsyncSession();
        var repository = new FetchProviderRepository(dbSession);
        var providerId = 999;

        // Act
        var result = await repository.GetProviderById(providerId);

        // Assert
        result.ShouldBeNull();
    }
}