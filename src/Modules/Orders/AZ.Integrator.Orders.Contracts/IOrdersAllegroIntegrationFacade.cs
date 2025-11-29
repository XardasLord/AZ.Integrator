using AZ.Integrator.Orders.Contracts.Dtos;

namespace AZ.Integrator.Orders.Contracts;

public interface IOrdersAllegroIntegrationFacade
{
    Task<AllegroShopInfoResponseDto> GetShopInfo(Guid tenantId, string accessToken, CancellationToken cancellationToken);
}