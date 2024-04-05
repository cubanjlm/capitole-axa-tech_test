using FluentValidation;

namespace ProviderService.Application.UseCases.Providers.ImportProviders;

public class ImportProvidersCommandValidator : AbstractValidator<ImportProvidersCommand>
{
    public ImportProvidersCommandValidator()
    {
        RuleFor(command => command.Providers).NotEmpty().WithMessage("Input data should not be empty.");
        RuleForEach(command => command.Providers).SetValidator(new ProviderCreationRequestValidator());
    }
}