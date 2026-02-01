using MediatR;
using ProvideService.Domain.Shared;

namespace ProviderService.Application.Abstractions.Messaging;

public interface ICommand : IBaseCommand, IRequest<Result>
{
}

public interface ICommand<TResponse> : IBaseCommand, IRequest<Result<TResponse>>
{
}

// a way to implement generic constrains that can target both interfaces.
// ex: custom mediator pipeline behaviors
public interface IBaseCommand
{
}