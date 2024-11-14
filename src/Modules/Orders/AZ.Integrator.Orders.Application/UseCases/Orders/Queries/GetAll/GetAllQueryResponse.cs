using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetAll;

public record GetAllQueryResponse(IEnumerable<GetOrderDetailsModelResponse> Orders, int Count, int TotalCount);