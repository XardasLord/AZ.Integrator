namespace AZ.Integrator.Domain.Aggregates.Order;

public enum OrderStatus
{
    New = 0,
    InvoicePrinted = 1,
    ShippingListGenerated = 2,
    Send = 3
}