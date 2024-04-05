using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using ProviderService.Application.UseCases.Providers.ImportProviders;
using ProvideService.Domain.Entities;
using ProvideService.Domain.Repositories;
using Shouldly;

namespace ProviderService.Application.Tests.UseCases.ImportProviders;

public class ImportProvidersCommandHandlerTests
{
    [Fact]
    public async Task GivenASetOfProviderToImport_WhenCallingImport_ThenProvidersGetSuccessfullyImported()
    {
        // Arrange
        var validator = new ImportProvidersCommandValidator();

        var writeProviderRepository = Substitute.For<IWriteProviderRepository>();
        var logger = new NullLogger<ImportProvidersCommandHandler>();

        var handler = new ImportProvidersCommandHandler(validator, writeProviderRepository, logger);

        var request = new ImportProvidersCommand
        {
            Providers = new List<ProviderCreationRequest>
            {
                new(1, "Test Provider", "Middle of nowhere, Betera", "vfh", DateTimeOffset.Now),
                new(2, "Test Provider Two", "Center street, Valencia", "rental", DateTimeOffset.Now),
                new(3, "Test Provider Three", "Downtown, Lliria", "rental", DateTimeOffset.Now)
            }
        };

        writeProviderRepository.ImportProviders(Arg.Any<List<Provider>>())
            .Returns(callInfo =>
            {
                var importedProviders = callInfo.Arg<List<Provider>>();
                // Simulate successful import by returning the id and names from the imported providers
                List<(int Id, string Name)> results =
                    importedProviders.Select(p => (p.CleanId(), p.Name)).ToList();
                return Task.FromResult(results);
            });

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(3);
    }

    [Fact]
    public async Task GivenNoImportPayload_WhenCallingImport_ThenABadRequestErrorIsReceived()
    {
        // Arrange
        const string validationErrorMessage = "Input data should not be empty.";
        var validator = new ImportProvidersCommandValidator();

        var writeProviderRepository = Substitute.For<IWriteProviderRepository>();
        var logger = new NullLogger<ImportProvidersCommandHandler>();

        var handler = new ImportProvidersCommandHandler(validator, writeProviderRepository, logger);

        var request = new ImportProvidersCommand();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.Code.ShouldBe("Error.BadRequest");
        result.Error.Message.ShouldContain(validationErrorMessage);
    }
}