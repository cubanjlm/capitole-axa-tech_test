using Microsoft.Extensions.Logging;
using ProviderService.Application.Abstractions.Messaging;
using ProvideService.Domain.Repositories;
using ProvideService.Domain.Shared;

namespace ProviderService.Application.UseCases.Providers.GetProviderById;

internal class GetProviderByIdQueryHandler : IQueryHandler<GetProviderByIdQuery, ProviderResponse>
{
    private readonly IFetchProviderRepository _providerReaderRepository;
    private readonly ILogger<GetProviderByIdQueryHandler> _logger;

    public GetProviderByIdQueryHandler(IFetchProviderRepository providerReaderRepository, ILogger<GetProviderByIdQueryHandler> logger)
    {
        _providerReaderRepository = providerReaderRepository;
        _logger = logger;
    }

    public async Task<Result<ProviderResponse>> Handle(GetProviderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var provider = await _providerReaderRepository.GetProviderById(request.ProviderId);
        if (provider is null)
        {
            _logger.LogError("The provider '{ProviderId}' not found.", request.ProviderId);
            return Result.Failure<ProviderResponse>(new Error("Error.NotFound",
                $"The provider '{request.ProviderId}' not found."));
        }

        var response = new ProviderResponse(provider.CleanId(), provider.Name, provider.PostalAddress.ToString(),
            provider.CreatedAt);

        return response;
    }
}