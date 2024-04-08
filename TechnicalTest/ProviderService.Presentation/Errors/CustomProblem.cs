using System.Text.Json.Serialization;

namespace ProviderService.Presentation.Errors;

public record CustomProblem(string ErrorCode, string ErrorDescription)
{
    [JsonPropertyName("errorCode")]
    public string ErrorCode { get; private init; } = ErrorCode;

    [JsonPropertyName("errorDescription")]

    public string ErrorDescription { get; private init; } = ErrorDescription;
}