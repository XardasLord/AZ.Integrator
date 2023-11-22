using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;

namespace AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;

public interface IAllegroService
{
    Task<IEnumerable<OrderEvent>> GetOrderEvents();
    Task<GetNewOrdersModelResponse> GetNewOrders(GetAllQueryFilters filters);
    Task<GetOrderDetailsModelResponse> GetOrderDetails(Guid orderId);
    Task ChangeStatus(Guid orderNumber, AllegroFulfillmentStatusEnum allegroFulfillmentStatusEnum, string allegroAccessToken);
}