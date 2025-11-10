using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.DpdShipment;

public sealed class DpdParcelTests
{
    [Fact]
    public void Register_WithValidData_ShouldCreateDpdParcel()
    {
        // Arrange
        var parcelNumber = new ParcelNumber(12345L);
        var waybill = new Waybill("WB123456");

        // Act
        var parcel = DpdParcel.Register(parcelNumber, waybill);

        // Assert
        parcel.Should().NotBeNull();
        parcel.Number.Should().Be(parcelNumber);
        parcel.Waybill.Should().Be(waybill);
    }

    [Fact]
    public void Register_ShouldCreateParcelWithCorrectProperties()
    {
        // Arrange
        const long parcelNumberValue = 99999L;
        const string waybillValue = "WB999999";
        var parcelNumber = new ParcelNumber(parcelNumberValue);
        var waybill = new Waybill(waybillValue);

        // Act
        var parcel = DpdParcel.Register(parcelNumber, waybill);

        // Assert
        parcel.Number.Value.Should().Be(parcelNumberValue);
        parcel.Waybill.Value.Should().Be(waybillValue);
    }

    [Fact]
    public void Register_MultipleInstances_ShouldCreateIndependentParcels()
    {
        // Arrange
        var parcelNumber1 = new ParcelNumber(11111L);
        var waybill1 = new Waybill("WB11111");
        var parcelNumber2 = new ParcelNumber(22222L);
        var waybill2 = new Waybill("WB22222");

        // Act
        var parcel1 = DpdParcel.Register(parcelNumber1, waybill1);
        var parcel2 = DpdParcel.Register(parcelNumber2, waybill2);

        // Assert
        parcel1.Should().NotBe(parcel2);
        parcel1.Number.Should().NotBe(parcel2.Number);
        parcel1.Waybill.Should().NotBe(parcel2.Waybill);
    }
}

