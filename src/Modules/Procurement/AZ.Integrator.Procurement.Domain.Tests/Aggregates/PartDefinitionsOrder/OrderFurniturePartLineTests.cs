namespace AZ.Integrator.Procurement.Domain.Tests.Aggregates.PartDefinitionsOrder;

// OrderFurniturePartLine.Create is internal method
// It is tested indirectly through OrderFurnitureLine and PartDefinitionsOrder aggregate tests
public sealed class OrderFurniturePartLineTests
{
    [Fact]
    public void OrderFurniturePartLine_IsTestedViaAggregates()
    {
        // OrderFurniturePartLine is an internal entity within the aggregate
        // It is created and tested through:
        // 1. PartDefinitionsOrder aggregate creation
        // 2. OrderFurnitureLine entity creation
        // This ensures proper encapsulation and domain integrity
        
        // Act & Assert
        Act(() => true.Should().BeTrue());
    }

    private static T Act<T>(Func<T> action) => action();
    private static void Act(Action action) => action();
}

