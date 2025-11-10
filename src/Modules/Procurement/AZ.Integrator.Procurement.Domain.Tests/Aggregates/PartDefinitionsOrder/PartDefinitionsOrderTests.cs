using AZ.Integrator.Domain.SeedWork;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder;
using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;
using AZ.Integrator.Procurement.Domain.Events;
using Bogus;
using PartDefinitionsOrderAggregate = AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.PartDefinitionsOrder;

namespace AZ.Integrator.Procurement.Domain.Tests.Aggregates.PartDefinitionsOrder;

public sealed class PartDefinitionsOrderTests
{
    private readonly FakeCurrentUser _currentUser = new();
    private readonly FakeCurrentDateTime _currentDateTime = new();
    private readonly FakeOrderNumberGenerator _orderNumberGenerator = new();

    [Fact]
    public void Create_WithValidData_ShouldCreatePartDefinitionsOrder()
    {
        // Arrange
        var supplierId = TestDataBuilder.CreateSupplierId();
        var furnitureModelLines = CreateTestFurnitureModelLines();
        const string additionalNotes = "Test order notes";

        // Act
        var result = Act(() => PartDefinitionsOrderAggregate.Create(
            supplierId,
            furnitureModelLines,
            additionalNotes,
            _orderNumberGenerator,
            _currentUser,
            _currentDateTime));

        // Assert
        result.Should().NotBeNull();
        result.SupplierId.Should().Be(supplierId);
        result.AdditionalNotes.Should().Be(additionalNotes);
        result.Status.Should().Be(OrderStatus.Registered);
        result.TenantId.Value.Should().Be(_currentUser.TenantId);
        result.FurnitureModelLines.Should().HaveCount(1);
    }

    [Fact]
    public void Create_ShouldGenerateOrderNumber()
    {
        // Arrange
        var supplierId = TestDataBuilder.CreateSupplierId();
        var furnitureModelLines = CreateTestFurnitureModelLines();
        const string additionalNotes = "Test notes";

        // Act
        var result = Act(() => PartDefinitionsOrderAggregate.Create(
            supplierId,
            furnitureModelLines,
            additionalNotes,
            _orderNumberGenerator,
            _currentUser,
            _currentDateTime));

        // Assert
        result.Number.Should().NotBeNull();
        result.Number.Value.Should().Be("ORD-000001");
    }

    [Fact]
    public void Create_ShouldSetCreationInformationCorrectly()
    {
        // Arrange
        var supplierId = TestDataBuilder.CreateSupplierId();
        var furnitureModelLines = CreateTestFurnitureModelLines();
        const string additionalNotes = "Test notes";
        var fixedDate = new DateTime(2025, 11, 10, 12, 0, 0);
        _currentDateTime.FixedDateTime = fixedDate;

        // Act
        var result = Act(() => PartDefinitionsOrderAggregate.Create(
            supplierId,
            furnitureModelLines,
            additionalNotes,
            _orderNumberGenerator,
            _currentUser,
            _currentDateTime));

        // Assert
        result.CreationInformation.CreatedAt.Should().Be(fixedDate);
        result.CreationInformation.CreatedBy.Should().Be(_currentUser.UserId);
        result.CreationInformation.TenantId.Value.Should().Be(_currentUser.TenantId);
    }

    [Fact]
    public void Create_ShouldRaisePartDefinitionsOrderRegisteredEvent()
    {
        // Arrange
        var supplierId = TestDataBuilder.CreateSupplierId();
        var furnitureModelLines = CreateTestFurnitureModelLines();
        const string additionalNotes = "Test notes";

        // Act
        var result = Act(() => PartDefinitionsOrderAggregate.Create(
            supplierId,
            furnitureModelLines,
            additionalNotes,
            _orderNumberGenerator,
            _currentUser,
            _currentDateTime));

        // Assert
        result.Events.Should().HaveCount(1);
        var domainEvent = result.Events.Should().ContainSingle()
            .Which.Should().BeOfType<PartDefinitionsOrderRegistered>().Subject;
        domainEvent.OrderNumber.Should().Be(result.Number.Value);
        domainEvent.SupplierId.Should().Be(supplierId.Value);
        domainEvent.TenantId.Should().Be(_currentUser.TenantId);
    }

    [Fact]
    public void Create_WithMultipleFurnitureModelLines_ShouldCreateAllLines()
    {
        // Arrange
        var supplierId = TestDataBuilder.CreateSupplierId();
        var furnitureModelLines = new List<FurnitureModelLineData>
        {
            CreateFurnitureModelLineData("CODE1", 1),
            CreateFurnitureModelLineData("CODE2", 2),
            CreateFurnitureModelLineData("CODE3", 3)
        };
        const string additionalNotes = "Multiple lines test";

        // Act
        var result = Act(() => PartDefinitionsOrderAggregate.Create(
            supplierId,
            furnitureModelLines,
            additionalNotes,
            _orderNumberGenerator,
            _currentUser,
            _currentDateTime));

        // Assert
        result.FurnitureModelLines.Should().HaveCount(3);
    }

    [Fact]
    public void Create_WithNullSupplierId_ShouldThrowArgumentNullException()
    {
        // Arrange
        var furnitureModelLines = CreateTestFurnitureModelLines();
        const string additionalNotes = "Test notes";

        // Act
        var act = () => Act(() => PartDefinitionsOrderAggregate.Create(
            null!,
            furnitureModelLines,
            additionalNotes,
            _orderNumberGenerator,
            _currentUser,
            _currentDateTime));

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MarkAsSent_FromRegisteredStatus_ShouldUpdateStatusToSent()
    {
        // Arrange
        var order = CreateTestOrder();

        // Act
        Act(() => order.MarkAsSent(_currentUser, _currentDateTime));

        // Assert
        order.Status.Should().Be(OrderStatus.Sent);
    }

    [Fact]
    public void MarkAsSent_ShouldRaisePartDefinitionsOrderSentEvent()
    {
        // Arrange
        var order = CreateTestOrder();
        order.ClearDomainEvents(); // Clear creation event

        // Act
        Act(() => order.MarkAsSent(_currentUser, _currentDateTime));

        // Assert
        order.Events.Should().HaveCount(1);
        var domainEvent = order.Events.Should().ContainSingle()
            .Which.Should().BeOfType<PartDefinitionsOrderSent>().Subject;
        domainEvent.OrderNumber.Should().Be(order.Number.Value);
        domainEvent.SupplierId.Should().Be(order.SupplierId.Value);
        domainEvent.TenantId.Should().Be(order.TenantId.Value);
    }

    [Fact]
    public void MarkAsSent_ShouldUpdateModificationInformation()
    {
        // Arrange
        var order = CreateTestOrder();
        var modificationDate = new Faker().Date.Past();
        _currentDateTime.FixedDateTime = modificationDate;

        // Act
        Act(() => order.MarkAsSent(_currentUser, _currentDateTime));

        // Assert
        order.ModificationInformation.ModifiedAt.Should().Be(modificationDate);
        order.ModificationInformation.ModifiedBy.Should().Be(_currentUser.UserId);
    }

    [Fact]
    public void MarkAsSent_FromSentStatus_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var order = CreateTestOrder();
        order.MarkAsSent(_currentUser, _currentDateTime);

        // Act
        var act = () => Act(() => order.MarkAsSent(_currentUser, _currentDateTime));

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Cannot mark order as sent from status*");
    }

    [Fact]
    public void FurnitureModelLines_ShouldBeReadOnly()
    {
        // Arrange
        var supplierId = TestDataBuilder.CreateSupplierId();
        var furnitureModelLines = CreateTestFurnitureModelLines();
        const string additionalNotes = "Test notes";

        // Act
        var result = Act(() => PartDefinitionsOrderAggregate.Create(
            supplierId,
            furnitureModelLines,
            additionalNotes,
            _orderNumberGenerator,
            _currentUser,
            _currentDateTime));

        // Assert
        result.FurnitureModelLines.Should().BeAssignableTo<IReadOnlyCollection<OrderFurnitureLine>>();
    }

    private PartDefinitionsOrderAggregate CreateTestOrder()
    {
        var order = PartDefinitionsOrderAggregate.Create(
            TestDataBuilder.CreateSupplierId(),
            CreateTestFurnitureModelLines(),
            "Test notes",
            _orderNumberGenerator,
            _currentUser,
            _currentDateTime);

        // Simulate Id assignment that would happen in repository on SaveChanges
        SetEntityId(order, new OrderId(TestDataBuilder.CreateRandomUInt()));

        return order;
    }

    private static void SetEntityId<TId>(Entity<TId> entity, TId id)
    {
        // Use reflection to set the Id property which is normally set by EF Core
        var idProperty = typeof(Entity<TId>).GetProperty(nameof(Entity<TId>.Id));
        idProperty?.SetValue(entity, id);
    }

    private static List<FurnitureModelLineData> CreateTestFurnitureModelLines()
    {
        return [CreateFurnitureModelLineData("FURNITURE-001", 1)];
    }

    private static FurnitureModelLineData CreateFurnitureModelLineData(string code, int version)
    {
        var partDefinitionLines = new List<PartDefinitionLineData>
        {
            new(
                null,
                new PartName("Top Panel"),
                Dimensions.Create(1000, 500, 18, EdgeBandingType.One, EdgeBandingType.Two),
                new Quantity(1),
                "Test part")
        };

        return new FurnitureModelLineData(
            code,
            version,
            new Quantity(5),
            partDefinitionLines);
    }

    private static T Act<T>(Func<T> action) => action();
    private static void Act(Action action) => action();
}

