using AZ.Integrator.Domain.Aggregates.Order.ValueObjects;
using AZ.Integrator.Domain.SeedWork;

namespace AZ.Integrator.Domain.Aggregates.Order;

public class Order : Entity, IAggregateRoot
{
    private OrderNumber _orderNumber;

    public OrderNumber OrderNumber => _orderNumber;
    
    private Order() { }
}