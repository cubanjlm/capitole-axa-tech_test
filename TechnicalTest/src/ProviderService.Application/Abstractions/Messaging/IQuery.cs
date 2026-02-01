using MediatR;
using ProvideService.Domain.Shared;

namespace ProviderService.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}