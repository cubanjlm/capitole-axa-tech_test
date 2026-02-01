using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using ProviderService.Application.UseCases.Providers.GetProviderById;
using ProvideService.Domain.Entities;
using ProvideService.Domain.Enums;
using ProvideService.Domain.Repositories;
using ProvideService.Domain.ValueObjects;
using Shouldly;

namespace ProviderService.Application.Tests.UseCases.GetProviderById;

public class GetProviderByIdQueryHandlerTests
{
    
    [Fact]
    public async Task GivenAProviderId_WhenFetchingAProvider_ThenTheExpectedProviderGetsRetrieved()
    {
        // Arrange
        var providerReaderRepository = Substitute.For<IFetchProviderRepository>();
        var logger = new NullLogger<GetProviderByIdQueryHandler>();// _loggerFactory.CreateLogger<IFetchProviderRepository>();
        var expectedProvider = new Provider(123, "Test Provider", PostalAddress.Create("medio del monte, Betera"),
            ProviderType.rental)
        {
            CreatedAt = DateTime.Now
        };
        providerReaderRepository.GetProviderById(Arg.Any<int>()).Returns(expectedProvider);

        var handler = new GetProviderByIdQueryHandler(providerReaderRepository, logger);

        var query = new GetProviderByIdQuery { ProviderId = 123 }; 

        providerReaderRepository.GetProviderById(query.ProviderId).Returns(expectedProvider);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldBe(expectedProvider.CleanId());
        result.Value.Name.ShouldBe(expectedProvider.Name);
        result.Value.PostalAddress.ShouldBe(expectedProvider.PostalAddress.ToString());
        result.Value.CreatedAt.ShouldBe(expectedProvider.CreatedAt);
    }

    [Fact]
    public async Task GivenAProviderId_WhenFetchingAProvider_ThenANotFoundFailureShouldBeReceived()
    {
        // Arrange
        var providerReaderRepository = Substitute.For<IFetchProviderRepository>();
        var logger = new NullLogger<GetProviderByIdQueryHandler>();

        var handler = new GetProviderByIdQueryHandler(providerReaderRepository, logger);

        var query = new GetProviderByIdQuery { ProviderId = 999 };

        providerReaderRepository.GetProviderById(query.ProviderId).Returns((Provider)null!);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Error.NotFound");
        result.Error.Message.ShouldBe($"The provider '{query.ProviderId}' not found.");
    }
}