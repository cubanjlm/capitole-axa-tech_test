using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProviderService.Application.UseCases.Providers.ImportProviders;

namespace ProviderService.Presentation.Endpoints.Providers;

public class ImportProvidersEndpoint : EndpointBaseAsync
    .WithRequest<List<ProviderInputDto>>
    .WithActionResult<List<ProviderOutputDto>>
{
    private readonly ISender _sender;
    private readonly ILogger<ImportProvidersEndpoint> _logger;

    public ImportProvidersEndpoint(ISender sender, ILogger<ImportProvidersEndpoint> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    [HttpPost("providers/import")]
    public override async Task<ActionResult<List<ProviderOutputDto>>> HandleAsync(
        [FromBody] List<ProviderInputDto> request,
        CancellationToken cancellationToken = new())
    {
        try
        {
            var domainProviders = request
                .Select(x => new ProviderCreationRequest(x.Id, x.Name, x.PostalAddress, x.Type, x.CreatedAt)).ToList();
            var importProvidersCommand = new ImportProvidersCommand(domainProviders);

            var operationResult = await _sender.Send(importProvidersCommand, cancellationToken);
            if (operationResult.IsFailure)
            {
                return BadRequest(operationResult.Error.Message);
            }

            var providerOutputDtos = operationResult.Value.Select(x => new ProviderOutputDto
            {
                Id = x.Id,
                Name = x.Name
            });
            return Ok(providerOutputDtos);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error server occured.");
            return Problem(detail: "An error server occured.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}