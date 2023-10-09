using AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;
using AZ.Integrator.Application.UseCases.Orders.Queries.GetAll;

namespace AZ.Integrator.Application.Common.ExternalServices.Allegro;

public interface IAllegroService
{
    Task<IEnumerable<OrderEvent>> GetOrderEvents();
    Task<GetNewOrdersModelResponse> GetNewOrders(GetAllQueryFilters filters);
    Task<GetOrderDetailsModelResponse> GetOrderDetails(Guid orderId);
    Task ChangeStatus(Guid orderNumber, AllegroFulfillmentStatusEnum allegroFulfillmentStatusEnum, string allegroAccessToken);
}