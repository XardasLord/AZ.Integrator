using AZ.Integrator.Domain.Aggregates.Order.ValueObjects;
using AZ.Integrator.Domain.SeedWork;

namespace AZ.Integrator.Domain.Aggregates.Order;

public class Order : Entity, IAggregateRoot
{
    private OrderNumber _number;
    private OrderStatus _status;

    public OrderNumber Number => _number;
    public OrderStatus Status => _status;
    
    private Order() { }
}