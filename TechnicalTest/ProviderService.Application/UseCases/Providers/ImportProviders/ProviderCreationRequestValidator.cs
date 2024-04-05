using FluentValidation;
using ProvideService.Domain.Enums;

namespace ProviderService.Application.UseCases.Providers.ImportProviders;

public class ProviderCreationRequestValidator : AbstractValidator<ProviderCreationRequest>
{
    public ProviderCreationRequestValidator()
    {
        RuleFor(request => request.ProviderId).GreaterThan(-1).WithMessage("The provider id must be grater than 0");
        RuleFor(request => request.PostalAddress).NotEmpty().WithMessage("The address should not be empty. Id {ProviderId}");
        RuleFor(request => request.Name).NotEmpty().WithMessage("Provider name cannot be empty. Id {ProviderId}");
        RuleFor(request => request.Type).IsEnumName(typeof(ProviderType))
            .WithMessage("The type is not allowed by the system");
    }
}