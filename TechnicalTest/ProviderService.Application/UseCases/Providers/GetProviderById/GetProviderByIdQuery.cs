using ProviderService.Application.Abstractions.Messaging;

namespace ProviderService.Application.UseCases.Providers.GetProviderById;

public record GetProviderByIdQuery(int ProviderId = default!) : IQuery<ProviderResponse>;

public record ProviderResponse(
    int Id = default!,
    string Name = default!,
    string PostalAddress = default!,
    DateTimeOffset CreatedAt = default!);