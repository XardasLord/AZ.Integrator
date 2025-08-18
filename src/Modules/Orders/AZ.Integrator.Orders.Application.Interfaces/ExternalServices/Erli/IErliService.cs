using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shared.Application.ExternalServices.Erli;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;

namespace AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Erli;

public interface IErliService
{
    Task<GetOrdersModelResponse> GetOrders(GetAllQueryFilters filters, TenantId tenantId);

    Task AssignTrackingNumber(
        string orderNumber,
        IEnumerable<string> trackingNumbers,
        string vendor,
        string deliveryTrackingStatus,
        string tenantId);
}