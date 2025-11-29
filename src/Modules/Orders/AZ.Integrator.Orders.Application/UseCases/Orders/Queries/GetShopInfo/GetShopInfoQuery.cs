using AZ.Integrator.Orders.Contracts.Dtos;
using Mediator;

namespace AZ.Integrator.Orders.Application.UseCases.Orders.Queries.GetShopInfo;

public record GetShopInfoQuery(Guid TenantId, string AccessToken) : IRequest<AllegroShopInfoResponseDto>;