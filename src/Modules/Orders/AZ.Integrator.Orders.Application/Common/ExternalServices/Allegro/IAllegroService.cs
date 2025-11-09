using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;

namespace AZ.Integrator.Orders.Application.Common.ExternalServices.Allegro;

public interface IAllegroService
{
    Task<IEnumerable<OrderEvent>> GetOrderEvents(Guid tenantId, string sourceSystemId);
    Task<GetNewOrdersModelResponse> GetOrders(GetAllQueryFilters filters, Guid tenantId, string sourceSystemId);
    Task<GetOrderDetailsModelResponse> GetOrderDetails(Guid orderId, Guid tenantId, string sourceSystemId);
    Task ChangeStatus(Guid orderNumber, AllegroFulfillmentStatusEnum allegroFulfillmentStatus, Guid tenantId, string sourceSystemId);
    Task AssignTrackingNumber(Guid orderNumber, IEnumerable<string> trackingNumbers, Guid tenantId, string sourceSystemId);
    Task AssignInvoice(Guid orderNumber, byte[] invoice, string invoiceNumber, Guid tenantId, string sourceSystemId);
}