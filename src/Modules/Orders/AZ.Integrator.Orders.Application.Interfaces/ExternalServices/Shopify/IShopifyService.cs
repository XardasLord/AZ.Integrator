using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Shopify;

namespace AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Shopify;

public interface IShopifyService
{
    Task<GetOrdersModelResponse> GetOrders(GetAllQueryFilters filters, TenantId tenantId);
}