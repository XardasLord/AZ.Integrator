using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;
using AZ.Integrator.Shipments.Domain.Events.DomainEvents.DpdShipment;
using DpdShipmentAggregate = AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.DpdShipment;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.DpdShipment;

public sealed class DpdShipmentTests
{
    private readonly FakeCurrentUser _currentUser = new();
    private readonly FakeCurrentDateTime _currentDateTime = new();

    [Fact]
    public void Create_WithValidData_ShouldCreateDpdShipment()
    {
        // Arrange
        var sessionNumber = new SessionNumber(12345L);
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var packages = new List<DpdPackage>
        {
            DpdPackage.Register(new PackageNumber(1L), new List<DpdParcel>
            {
                DpdParcel.Register(new ParcelNumber(1L), new Waybill("WB001"))
            })
        };

        // Act
        var shipment = DpdShipmentAggregate.Create(
            sessionNumber,
            packages,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        shipment.Should().NotBeNull();
        shipment.SessionNumber.Should().Be(sessionNumber);
        shipment.ExternalOrderNumber.Should().Be(externalOrderNumber);
        shipment.CreationInformation.TenantId.Should().Be(tenantId);
        shipment.CreationInformation.SourceSystemId.Should().Be(sourceSystemId);
        shipment.Packages.Should().HaveCount(1);
    }

    [Fact]
    public void Create_ShouldSetCreationInformationCorrectly()
    {
        // Arrange
        var sessionNumber = new SessionNumber(99999L);
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var packages = new List<DpdPackage>();
        var fixedDate = new DateTime(2025, 11, 10, 12, 0, 0);
        _currentDateTime.FixedDateTime = fixedDate;

        // Act
        var shipment = DpdShipmentAggregate.Create(
            sessionNumber,
            packages,
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
    public void Create_ShouldRaiseDpdShipmentRegisteredDomainEvent()
    {
        // Arrange
        var sessionNumber = new SessionNumber(55555L);
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var packages = new List<DpdPackage>();

        // Act
        var shipment = DpdShipmentAggregate.Create(
            sessionNumber,
            packages,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        shipment.Events.Should().HaveCount(1);
        var domainEvent = shipment.Events.Should().ContainSingle()
            .Which.Should().BeOfType<DpdShipmentRegistered>().Subject;
        domainEvent.SessionNumber.Should().Be(sessionNumber.Value);
        domainEvent.AllegroOrderNumber.Should().Be(externalOrderNumber.Value);
        domainEvent.TenantId.Should().Be(tenantId);
        domainEvent.SourceSystemId.Should().Be(sourceSystemId);
    }

    [Fact]
    public void Create_WithMultiplePackages_ShouldContainAllPackages()
    {
        // Arrange
        var sessionNumber = new SessionNumber(77777L);
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var package1 = DpdPackage.Register(new PackageNumber(1L), new List<DpdParcel>());
        var package2 = DpdPackage.Register(new PackageNumber(2L), new List<DpdParcel>());
        var package3 = DpdPackage.Register(new PackageNumber(3L), new List<DpdParcel>());
        var packages = new List<DpdPackage> { package1, package2, package3 };

        // Act
        var shipment = DpdShipmentAggregate.Create(
            sessionNumber,
            packages,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        shipment.Packages.Should().HaveCount(3);
        shipment.Packages.Should().Contain(package1);
        shipment.Packages.Should().Contain(package2);
        shipment.Packages.Should().Contain(package3);
    }

    [Fact]
    public void Create_WithEmptyPackages_ShouldCreateShipmentWithNoPackages()
    {
        // Arrange
        var sessionNumber = new SessionNumber(88888L);
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var packages = new List<DpdPackage>();

        // Act
        var shipment = DpdShipmentAggregate.Create(
            sessionNumber,
            packages,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        shipment.Packages.Should().BeEmpty();
    }

    [Fact]
    public void Packages_ShouldBeReadOnly()
    {
        // Arrange
        var sessionNumber = new SessionNumber(44444L);
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var packages = new List<DpdPackage>
        {
            DpdPackage.Register(new PackageNumber(1L), new List<DpdParcel>())
        };

        // Act
        var shipment = DpdShipmentAggregate.Create(
            sessionNumber,
            packages,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        shipment.Packages.Should().BeAssignableTo<IReadOnlyCollection<DpdPackage>>();
    }

    [Fact]
    public void Create_ShouldGenerateCorrelationId()
    {
        // Arrange
        var sessionNumber = new SessionNumber(33333L);
        var externalOrderNumber = TestDataBuilder.CreateExternalOrderNumber();
        var tenantId = TestDataBuilder.CreateTenantId();
        var sourceSystemId = TestDataBuilder.CreateSourceSystemId();
        var packages = new List<DpdPackage>();

        // Act
        var shipment = DpdShipmentAggregate.Create(
            sessionNumber,
            packages,
            externalOrderNumber,
            tenantId,
            sourceSystemId,
            _currentUser,
            _currentDateTime);

        // Assert
        var domainEvent = shipment.Events.OfType<DpdShipmentRegistered>().First();
        domainEvent.CorrelationId.Should().NotBeNullOrWhiteSpace();
    }
}

