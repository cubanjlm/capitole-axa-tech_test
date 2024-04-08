using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProviderService.Application.UseCases.Providers.ImportProviders;
using ProviderService.Presentation.Errors;

namespace ProviderService.Presentation.Endpoints.Providers;

public class ImportProvidersEndpoint : EndpointBaseAsync
    .WithRequest<List<ProviderInputDto>>
    .WithActionResult<List<ProviderOutputDto>>
{
    private readonly ISender _sender;

    public ImportProvidersEndpoint(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("providers/import")]
    public override async Task<ActionResult<List<ProviderOutputDto>>> HandleAsync(
        [FromBody] List<ProviderInputDto> request,
        CancellationToken cancellationToken = new())
    {
        var domainProviders = request
            .Select(x => new ProviderCreationRequest(x.Id, x.Name, x.PostalAddress, x.Type, x.CreatedAt)).ToList();
        var importProvidersCommand = new ImportProvidersCommand(domainProviders);

        var operationResult = await _sender.Send(importProvidersCommand, cancellationToken);
        if (operationResult.IsFailure)
        {
            var problem = new CustomProblem(operationResult.Error.Code, operationResult.Error.Message);
            return BadRequest(problem);
        }

        var providerOutputDtos = operationResult.Value.Select(x => new ProviderOutputDto
        {
            Id = x.Id,
            Name = x.Name
        });
        return Ok(providerOutputDtos);
    }
}