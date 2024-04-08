using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProviderService.Application.UseCases.Providers.GetProviderById;
using ProviderService.Presentation.Errors;

namespace ProviderService.Presentation.Endpoints.Providers;

public class GetProviderByIdEndpoint : EndpointBaseAsync
    .WithRequest<GetProviderByIdRequest>
    .WithActionResult<ProviderDto>
{
    private readonly ISender _sender;

    public GetProviderByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("providers/{id:int}")]
    public override async Task<ActionResult<ProviderDto>> HandleAsync([FromRoute] GetProviderByIdRequest request,
        CancellationToken cancellationToken = new())
    {
        var query = new GetProviderByIdQuery(request.ProviderId);
        var operationResult = await _sender.Send(query, cancellationToken);

        if (operationResult.IsFailure)
        {
            var problem = new CustomProblem(operationResult.Error.Code, operationResult.Error.Message);
            return NotFound(problem);
        }

        var (id, name, postalAddress, createdAt) = operationResult.Value;
        var response = new ProviderDto
        {
            Id = id,
            Name = name,
            PostalAddress = postalAddress,
            CreatedAt = createdAt.UtcDateTime
        };

        return Ok(response);
    }
}

public record GetProviderByIdRequest
{
    [FromRoute(Name = "id")] public int ProviderId { get; init; }
}