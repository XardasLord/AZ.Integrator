using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using Mediator;

namespace AZ.Integrator.Application.UseCases.Orders.Queries.GetAll;

public record GetAllQuery : IQuery<GetAllQueryResponse>;