using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.DomainServices;

public interface IOrderNumberGenerator
{
    OrderNumber Generate();
}