using FluentValidation;
using Microsoft.Extensions.Logging;
using ProviderService.Application.Abstractions.Messaging;
using ProvideService.Domain.Entities;
using ProvideService.Domain.Enums;
using ProvideService.Domain.Repositories;
using ProvideService.Domain.Shared;
using ProvideService.Domain.ValueObjects;

namespace ProviderService.Application.UseCases.Providers.ImportProviders;

internal class ImportProvidersCommandHandler : ICommandHandler<ImportProvidersCommand, List<ProviderResp>>
{
    private readonly IValidator<ImportProvidersCommand> _validator;
    private readonly IWriteProviderRepository _writeProviderRepository;
    private readonly ILogger<ImportProvidersCommandHandler> _logger;

    public ImportProvidersCommandHandler(IValidator<ImportProvidersCommand> validator,
        IWriteProviderRepository writeProviderRepository,
        ILogger<ImportProvidersCommandHandler> logger)
    {
        _validator = validator;
        _writeProviderRepository = writeProviderRepository;
        _logger = logger;
    }

    public async Task<Result<List<ProviderResp>>> Handle(ImportProvidersCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
        }
        catch (ValidationException e)
        {
            _logger.LogError(e, "Some validation error were detected");
            return Result.Failure<List<ProviderResp>>(new Error("Error.BadRequest", e.Message));
        }

        //todo: refactor => move to a domain service/utility
        var importList = new List<Provider>();

        foreach (var requestProvider in request.Providers)
        {
            var resultAddress = PostalAddress.Create(requestProvider.PostalAddress);
            ProviderType.TryParse(requestProvider.Type, out ProviderType parsedType);


            var provider = new Provider(requestProvider.ProviderId,
                requestProvider.Name, resultAddress, parsedType)
            {
                CreatedAt = requestProvider.CreatedAt
            };
            importList.Add(provider);
        }
        //until here

        var succeedImportedReference = await _writeProviderRepository.ImportProviders(importList);

        var providerRespList = succeedImportedReference.Select(x => new ProviderResp(x.Id, x.Name)).ToList();
        return Result.Success(providerRespList);
    }
}