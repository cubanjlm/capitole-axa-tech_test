﻿using MediatR;
using ProvideService.Domain.Shared;

namespace ProviderService.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse>: IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
{
}