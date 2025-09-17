using AZ.Integrator.Orders.Contracts.Dtos;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetAll;

public record GetAllQueryResponse(IEnumerable<OrderDetailsDto> Orders, int Count, int TotalCount);