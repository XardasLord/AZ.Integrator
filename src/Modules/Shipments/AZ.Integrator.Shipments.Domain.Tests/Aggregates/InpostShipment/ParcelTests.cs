using AZ.Integrator.Domain.SharedKernel.ValueObjects;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.InpostShipment;

public sealed class ParcelTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateParcel()
    {
        // Arrange
        var tenantId = new TenantId(Guid.NewGuid());
        var trackingNumber = new TrackingNumber("TRK123456");

        // Act
        var parcel = Parcel.Create(tenantId, trackingNumber);

        // Assert
        parcel.Should().NotBeNull();
        parcel.TenantId.Should().Be(tenantId);
        parcel.TrackingNumber.Should().Be(trackingNumber);
    }

    [Fact]
    public void Create_ShouldCreateParcelWithCorrectProperties()
    {
        // Arrange
        var tenantIdValue = Guid.NewGuid();
        var tenantId = new TenantId(tenantIdValue);
        const string trackingNumberValue = "TRK987654";
        var trackingNumber = new TrackingNumber(trackingNumberValue);

        // Act
        var parcel = Parcel.Create(tenantId, trackingNumber);

        // Assert
        parcel.TenantId.Value.Should().Be(tenantIdValue);
        parcel.TrackingNumber.Value.Should().Be(trackingNumberValue);
    }

    [Fact]
    public void Create_MultipleInstances_ShouldCreateIndependentParcels()
    {
        // Arrange
        var tenantId1 = new TenantId(Guid.NewGuid());
        var trackingNumber1 = new TrackingNumber("TRK111111");
        var tenantId2 = new TenantId(Guid.NewGuid());
        var trackingNumber2 = new TrackingNumber("TRK222222");

        // Act
        var parcel1 = Parcel.Create(tenantId1, trackingNumber1);
        var parcel2 = Parcel.Create(tenantId2, trackingNumber2);

        // Assert
        parcel1.Should().NotBe(parcel2);
        parcel1.TenantId.Should().NotBe(parcel2.TenantId);
        parcel1.TrackingNumber.Should().NotBe(parcel2.TrackingNumber);
    }
}

