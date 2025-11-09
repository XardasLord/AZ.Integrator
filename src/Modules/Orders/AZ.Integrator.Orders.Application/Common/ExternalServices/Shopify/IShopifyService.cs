using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Shopify;
using AZ.Integrator.Shared.Application.ExternalServices.Shopify.GraphqlResponses;

namespace AZ.Integrator.Orders.Application.Common.ExternalServices.Shopify;

public interface IShopifyService
{
    Task<GetOrdersModelResponse> GetOrders(GetAllQueryFilters filters, TenantId tenantId, SourceSystemId sourceSystemId);
    Task<Order> GetOrderDetails(string orderNumber, TenantId tenantId, SourceSystemId sourceSystemId);
    
    Task AssignTrackingNumber(
        string orderNumber,
        IEnumerable<string> trackingNumbers,
        string vendor,
        Guid tenantId,
        string sourceSystemId);
}