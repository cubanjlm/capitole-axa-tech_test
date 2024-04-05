using System.Text.Json.Serialization;

namespace ProviderService.Presentation.Endpoints;

public record ProviderOutputDto
{
    [JsonPropertyName("provider_id")] public int Id { get; init; }
    public string Name { get; init; }
}

public record ProviderDto : ProviderOutputDto 
{
    [JsonPropertyName("postal_address")] public string PostalAddress { get; init; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; init; }
}

public record ProviderInputDto : ProviderDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}