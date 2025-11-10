using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.DpdShipment;

public sealed class DpdPackageTests
{
    [Fact]
    public void Register_WithValidData_ShouldCreateDpdPackage()
    {
        // Arrange
        var packageNumber = new PackageNumber(98765L);
        var parcels = new List<DpdParcel>
        {
            DpdParcel.Register(new ParcelNumber(1L), new Waybill("WB001")),
            DpdParcel.Register(new ParcelNumber(2L), new Waybill("WB002"))
        };

        // Act
        var package = DpdPackage.Register(packageNumber, parcels);

        // Assert
        package.Should().NotBeNull();
        package.Number.Should().Be(packageNumber);
        package.Parcels.Should().HaveCount(2);
    }

    [Fact]
    public void Register_WithEmptyParcels_ShouldCreatePackageWithNoParcels()
    {
        // Arrange
        var packageNumber = new PackageNumber(98765L);
        var parcels = new List<DpdParcel>();

        // Act
        var package = DpdPackage.Register(packageNumber, parcels);

        // Assert
        package.Should().NotBeNull();
        package.Number.Should().Be(packageNumber);
        package.Parcels.Should().BeEmpty();
    }

    [Fact]
    public void Register_WithMultipleParcels_ShouldContainAllParcels()
    {
        // Arrange
        var packageNumber = new PackageNumber(55555L);
        var parcel1 = DpdParcel.Register(new ParcelNumber(1L), new Waybill("WB001"));
        var parcel2 = DpdParcel.Register(new ParcelNumber(2L), new Waybill("WB002"));
        var parcel3 = DpdParcel.Register(new ParcelNumber(3L), new Waybill("WB003"));
        var parcels = new List<DpdParcel> { parcel1, parcel2, parcel3 };

        // Act
        var package = DpdPackage.Register(packageNumber, parcels);

        // Assert
        package.Parcels.Should().Contain(parcel1);
        package.Parcels.Should().Contain(parcel2);
        package.Parcels.Should().Contain(parcel3);
        package.Parcels.Should().HaveCount(3);
    }

    [Fact]
    public void Parcels_ShouldBeReadOnly()
    {
        // Arrange
        var packageNumber = new PackageNumber(12345L);
        var parcels = new List<DpdParcel>
        {
            DpdParcel.Register(new ParcelNumber(1L), new Waybill("WB001"))
        };
        var package = DpdPackage.Register(packageNumber, parcels);

        // Act & Assert
        package.Parcels.Should().BeAssignableTo<IReadOnlyCollection<DpdParcel>>();
    }

    [Fact]
    public void Register_ShouldPreserveParcelOrder()
    {
        // Arrange
        var packageNumber = new PackageNumber(99999L);
        var parcel1 = DpdParcel.Register(new ParcelNumber(1L), new Waybill("WB001"));
        var parcel2 = DpdParcel.Register(new ParcelNumber(2L), new Waybill("WB002"));
        var parcel3 = DpdParcel.Register(new ParcelNumber(3L), new Waybill("WB003"));
        var parcels = new List<DpdParcel> { parcel1, parcel2, parcel3 };

        // Act
        var package = DpdPackage.Register(packageNumber, parcels);

        // Assert
        package.Parcels.Should().ContainInOrder(parcel1, parcel2, parcel3);
    }
}

