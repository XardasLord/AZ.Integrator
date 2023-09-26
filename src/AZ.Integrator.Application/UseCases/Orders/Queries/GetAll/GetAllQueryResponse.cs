using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Application.UseCases.Orders.Queries.GetAll;

public record GetAllQueryResponse(IEnumerable<OrderListDto> Orders, int TotalCount);