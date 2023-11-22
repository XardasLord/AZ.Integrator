using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using MediatR;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetAll;

public record GetAllQuery(GetAllQueryFilters Filters) : IRequest<GetAllQueryResponse>;