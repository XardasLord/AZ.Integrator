using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shared.Application.ExternalServices.Erli;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;

namespace AZ.Integrator.Orders.Application.Common.ExternalServices.Erli;

public interface IErliService
{
    Task<GetOrdersModelResponse> GetOrders(GetAllQueryFilters filters, TenantId tenantId, SourceSystemId sourceSystemId);
    Task<Order> GetOrderDetails(string orderId, TenantId tenantId, SourceSystemId sourceSystemId);

    Task AssignTrackingNumber(
        string orderNumber,
        IEnumerable<string> trackingNumbers,
        string vendor,
        string deliveryTrackingStatus,
        Guid tenantId, 
        string sourceSystemId);
}