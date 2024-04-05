using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProviderService.Application.UseCases.Providers.GetProviderById;

namespace ProviderService.Presentation.Endpoints.Providers;

public class GetProviderByIdEndpoint : EndpointBaseAsync
    .WithRequest<GetProviderByIdRequest>
    .WithActionResult<ProviderDto>
{
    private readonly ISender _sender;
    private readonly ILogger<GetProviderByIdEndpoint> _logger;

    public GetProviderByIdEndpoint(ISender sender, ILogger<GetProviderByIdEndpoint> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    [HttpGet("providers/{id:int}")]
    public override async Task<ActionResult<ProviderDto>> HandleAsync([FromRoute] GetProviderByIdRequest request,
        CancellationToken cancellationToken = new())
    {
        try
        {
            var query = new GetProviderByIdQuery(request.ProviderId);
            var operationResult = await _sender.Send(query, cancellationToken);

            if (operationResult.IsFailure)
            {
                return NotFound(operationResult.Error.Message);
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
        catch (Exception e)
        {
            _logger.LogError(e, "An error server occured.");
            return Problem(detail: "An error server occured.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}

public record GetProviderByIdRequest
{
    [FromRoute(Name = "id")] public int ProviderId { get; init; }
}