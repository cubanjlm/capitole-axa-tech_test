using System.Text.Json.Serialization;

namespace ProviderService.Presentation.Endpoints;

public record ProviderOutputDto
{
    [JsonPropertyName("provider_id")] public int Id { get; init; }
    public required string Name { get; init; }
}

public record ProviderDto : ProviderOutputDto 
{
    [JsonPropertyName("postal_address")] public required string PostalAddress { get; init; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; init; }
}

public record ProviderInputDto : ProviderDto
{
    [JsonPropertyName("type")]
    public required string Type { get; set; }
}