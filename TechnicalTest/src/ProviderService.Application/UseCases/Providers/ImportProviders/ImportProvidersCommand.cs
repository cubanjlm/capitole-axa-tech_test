using ProviderService.Application.Abstractions.Messaging;
using ProvideService.Domain.Shared;

namespace ProviderService.Application.UseCases.Providers.ImportProviders;

public sealed record ImportProvidersCommand(List<ProviderCreationRequest> Providers = default!) : ICommand<List<ProviderResp>>;

public sealed record ProviderCreationRequest(
    int ProviderId,
    string Name,
    string PostalAddress,
    string Type,
    DateTimeOffset CreatedAt);
    
public record ProviderResp(int Id = default!, string Name = default!);