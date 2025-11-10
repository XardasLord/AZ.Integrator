using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.DomainServices;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Tests.TestHelpers;

internal sealed class FakeOrderNumberGenerator : IOrderNumberGenerator
{
    private int _counter = 1;

    public OrderNumber Generate()
    {
        return new OrderNumber($"ORD-{_counter++:D6}");
    }

    public void Reset() => _counter = 1;
}

