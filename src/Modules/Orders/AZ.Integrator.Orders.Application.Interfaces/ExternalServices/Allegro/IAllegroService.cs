﻿using AZ.Integrator.Shared.Application.ExternalServices.Allegro.Models;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;

namespace AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Allegro;

public interface IAllegroService
{
    Task<IEnumerable<OrderEvent>> GetOrderEvents();
    Task<GetNewOrdersModelResponse> GetOrders(GetAllQueryFilters filters);
    Task<GetOrderDetailsModelResponse> GetOrderDetails(Guid orderId);
    Task<GetOffersResponse> GetOffers(GetProductTagsQueryFilters filters);
    Task ChangeStatus(Guid orderNumber, AllegroFulfillmentStatusEnum allegroFulfillmentStatus, string tenantId);
    Task AssignTrackingNumber(Guid orderNumber, IEnumerable<string> trackingNumbers, string tenantId);
}