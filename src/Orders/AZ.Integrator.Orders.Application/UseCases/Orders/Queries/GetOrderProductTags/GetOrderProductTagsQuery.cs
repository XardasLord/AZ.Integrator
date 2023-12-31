﻿using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetOrderProductTags;

public record GetOrderProductTagsQuery(Guid OrderId) : IRequest<IEnumerable<string>>;