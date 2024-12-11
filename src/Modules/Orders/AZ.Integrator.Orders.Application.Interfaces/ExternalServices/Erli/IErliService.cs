﻿using AZ.Integrator.Shared.Application.ExternalServices.Erli;
using AZ.Integrator.Shared.Application.ExternalServices.Shared.Models;

namespace AZ.Integrator.Orders.Application.Interfaces.ExternalServices.Erli;

public interface IErliService
{
    Task<GetOrdersModelResponse> GetOrders(GetAllQueryFilters filters);
    Task<GetProductsModelResponse> GetProducts(GetProductTagsQueryFilters filters);

    Task AssignTrackingNumber(
        string orderNumber,
        IEnumerable<string> trackingNumbers,
        string vendor,
        string deliveryTrackingStatus,
        string tenantId);
}