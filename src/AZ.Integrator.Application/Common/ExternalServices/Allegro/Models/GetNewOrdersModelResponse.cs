﻿namespace AZ.Integrator.Application.Common.ExternalServices.Allegro.Models;

public class GetNewOrdersModelResponse
{
    public IEnumerable<GetOrderDetailsModelResponse> CheckoutForms { get; set; }
    public int Count { get; set; }
    public int TotalCount { get; set; }
}