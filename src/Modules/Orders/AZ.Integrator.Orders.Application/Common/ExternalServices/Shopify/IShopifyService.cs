using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Shopify;
using AZ.Integrator.Shared.Application.ExternalServices.Shopify.GraphqlResponses;

namespace AZ.Integrator.Orders.Application.Common.ExternalServices.Shopify;

public interface IShopifyService
{
    Task<GetOrdersModelResponse> GetOrders(GetAllQueryFilters filters, TenantId tenantId);
    Task<Order> GetOrderDetails(string orderNumber, TenantId tenantId);
    
    Task AssignTrackingNumber(
        string orderNumber,
        IEnumerable<string> trackingNumbers,
        string vendor,
        string tenantId);
}