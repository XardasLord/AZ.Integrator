using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.DomainServices;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;

namespace AZ.Integrator.Procurement.Infrastructure.DomainServices;

public class OrderNumberGenerator : IOrderNumberGenerator
{
    public OrderNumber Generate()
    {
        var now = DateTime.UtcNow;
        var shortGuid = Guid.NewGuid().ToString("N")[..4];
        
        return new OrderNumber($"ZAM-{now:yyyy-MM-dd}-{shortGuid}");
    }
}