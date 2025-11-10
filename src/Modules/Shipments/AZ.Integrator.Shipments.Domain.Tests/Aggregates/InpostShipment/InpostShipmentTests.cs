using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Events;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;
using InpostShipmentAggregate = AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.InpostShipment;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.InpostShipment;

public sealed class InpostShipmentTests
{
    private readonly FakeCurrentUser _currentUser = new();
    private readonly FakeCurrentDateTime _currentDateTime = new();

    [Fact]
    public void Create_WithValidData_ShouldCreateInpostShipment()
    {
        // Arrange
        var shipmentNumber = new ShipmentNumber("SHIP123456");
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();

        // Act
        var shipment = InpostShipmentAggregate.Create(
            shipmentNumber,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        shipment.Should().NotBeNull();
        shipment.Number.Should().Be(shipmentNumber);
        shipment.ExternalOrderNumber.Should().Be(externalOrderNumber);
        shipment.CreationInformation.TenantId.Should().Be(tenantId);
        shipment.CreationInformation.SourceSystemId.Should().Be(sourceSystemId);
    }

    [Fact]
    public void Create_ShouldSetCreationInformationCorrectly()
    {
        // Arrange
        var shipmentNumber = new ShipmentNumber("SHIP999999");
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var fixedDate = new DateTime(2025, 11, 10, 12, 0, 0);
        _currentDateTime.FixedDateTime = fixedDate;

        // Act
        var shipment = InpostShipmentAggregate.Create(
            shipmentNumber,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        shipment.CreationInformation.CreatedAt.Should().Be(fixedDate);
        shipment.CreationInformation.CreatedBy.Should().Be(_currentUser.UserId);
        shipment.CreationInformation.TenantId.Should().Be(tenantId);
        shipment.CreationInformation.SourceSystemId.Should().Be(sourceSystemId);
    }

    [Fact]
    public void Create_ShouldRaiseInpostShipmentRegisteredDomainEvent()
    {
        // Arrange
        var shipmentNumber = new ShipmentNumber("SHIP555555");
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();

        // Act
        var shipment = InpostShipmentAggregate.Create(
            shipmentNumber,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        shipment.Events.Should().HaveCount(1);
        var domainEvent = shipment.Events.Should().ContainSingle()
            .Which.Should().BeOfType<InpostShipmentRegistered>().Subject;
        domainEvent.ShipmentNumber.Should().Be(shipmentNumber.Value);
        domainEvent.ExternalOrderNumber.Should().Be(externalOrderNumber.Value);
        domainEvent.TenantId.Should().Be(tenantId.Value);
        domainEvent.SourceSystemId.Should().Be(sourceSystemId.Value);
    }

    [Fact]
    public void Create_ShouldInitializeWithEmptyParcels()
    {
        // Arrange
        var shipmentNumber = new ShipmentNumber("SHIP111111");
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();

        // Act
        var shipment = InpostShipmentAggregate.Create(
            shipmentNumber,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        shipment.Parcels.Should().BeEmpty();
    }

    [Fact]
    public void SetTrackingNumber_WithValidData_ShouldAddParcels()
    {
        // Arrange
        var shipmentNumber = new ShipmentNumber("SHIP222222");
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var shipment = InpostShipmentAggregate.Create(
            shipmentNumber,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);
        var trackingNumbers = new List<TrackingNumber>
        {
            new TrackingNumber("TRK001"),
            new TrackingNumber("TRK002"),
            new TrackingNumber("TRK003")
        };

        // Act
        shipment.SetTrackingNumber(trackingNumbers, tenantId);

        // Assert
        shipment.Parcels.Should().HaveCount(3);
        shipment.Parcels.Select(p => p.TrackingNumber.Value)
            .Should().Contain(new[] { "TRK001", "TRK002", "TRK003" });
    }

    [Fact]
    public void SetTrackingNumber_ShouldRaiseInpostTrackingNumbersAssignedDomainEvent()
    {
        // Arrange
        var shipmentNumber = new ShipmentNumber("SHIP333333");
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var shipment = InpostShipmentAggregate.Create(
            shipmentNumber,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);
        shipment.ClearDomainEvents(); // Clear creation event
        var trackingNumbers = new List<TrackingNumber>
        {
            new TrackingNumber("TRK111"),
            new TrackingNumber("TRK222")
        };

        // Act
        shipment.SetTrackingNumber(trackingNumbers, tenantId);

        // Assert
        shipment.Events.Should().HaveCount(1);
        var domainEvent = shipment.Events.Should().ContainSingle()
            .Which.Should().BeOfType<InpostTrackingNumbersAssigned>().Subject;
        domainEvent.ShipmentNumber.Should().Be(shipmentNumber.Value);
        domainEvent.TrackingNumbers.Should().BeEquivalentTo(new[] { "TRK111", "TRK222" });
        domainEvent.ExternalOrderNumber.Should().Be(externalOrderNumber.Value);
        domainEvent.TenantId.Should().Be(tenantId.Value);
    }

    [Fact]
    public void SetTrackingNumber_WithSingleTrackingNumber_ShouldAddOneParcel()
    {
        // Arrange
        var shipmentNumber = new ShipmentNumber("SHIP444444");
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var shipment = InpostShipmentAggregate.Create(
            shipmentNumber,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);
        var trackingNumbers = new List<TrackingNumber>
        {
            new TrackingNumber("TRKSINGULAR")
        };

        // Act
        shipment.SetTrackingNumber(trackingNumbers, tenantId);

        // Assert
        shipment.Parcels.Should().HaveCount(1);
        shipment.Parcels.First().TrackingNumber.Value.Should().Be("TRKSINGULAR");
    }

    [Fact]
    public void SetTrackingNumber_MultipleTimes_ShouldAccumulateParcels()
    {
        // Arrange
        var shipmentNumber = new ShipmentNumber("SHIP555555");
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var shipment = InpostShipmentAggregate.Create(
            shipmentNumber,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);
        var trackingNumbers1 = new List<TrackingNumber>
        {
            new TrackingNumber("TRK001")
        };
        var trackingNumbers2 = new List<TrackingNumber>
        {
            new TrackingNumber("TRK002")
        };

        // Act
        shipment.SetTrackingNumber(trackingNumbers1, tenantId);
        shipment.SetTrackingNumber(trackingNumbers2, tenantId);

        // Assert
        shipment.Parcels.Should().HaveCount(2);
        shipment.Parcels.Select(p => p.TrackingNumber.Value)
            .Should().Contain(new[] { "TRK001", "TRK002" });
    }

    [Fact]
    public void Parcels_ShouldBeReadOnly()
    {
        // Arrange
        var shipmentNumber = new ShipmentNumber("SHIP666666");
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();

        // Act
        var shipment = InpostShipmentAggregate.Create(
            shipmentNumber,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        shipment.Parcels.Should().BeAssignableTo<IReadOnlyCollection<Parcel>>();
    }

    [Fact]
    public void SetTrackingNumber_ShouldSetCorrectTenantIdToParcels()
    {
        // Arrange
        var shipmentNumber = new ShipmentNumber("SHIP777777");
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var shipment = InpostShipmentAggregate.Create(
            shipmentNumber,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);
        var trackingNumbers = new List<TrackingNumber>
        {
            new TrackingNumber("TRK123")
        };

        // Act
        shipment.SetTrackingNumber(trackingNumbers, tenantId);

        // Assert
        shipment.Parcels.Should().AllSatisfy(p => p.TenantId.Should().Be(tenantId));
    }

    [Fact]
    public void Create_ShouldGenerateCorrelationId()
    {
        // Arrange
        var shipmentNumber = new ShipmentNumber("SHIP888888");
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();

        // Act
        var shipment = InpostShipmentAggregate.Create(
            shipmentNumber,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        var domainEvent = shipment.Events.OfType<InpostShipmentRegistered>().First();
        domainEvent.CorrelationId.Should().NotBeNullOrWhiteSpace();
    }
}
