using Ardalis.GuardClauses;
using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.Exceptions;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Exceptions;
using AZ.Integrator.Shipments.Domain.Extensions;

namespace AZ.Integrator.Shipments.Domain.Tests.Extensions;

public sealed class GuardExtensionsTests
{
    [Fact]
    public void InpostTrackingNumber_WithValidValue_ShouldReturnValue()
    {
        // Arrange
        const string validTrackingNumber = "TRK123456";

        // Act
        var result = Guard.Against.InpostTrackingNumber(validTrackingNumber);

        // Assert
        result.Should().Be(validTrackingNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void InpostTrackingNumber_WithInvalidValue_ShouldThrowInvalidTrackingNumberException(string invalidValue)
    {
        // Act
        var act = () => Guard.Against.InpostTrackingNumber(invalidValue);

        // Assert
        act.Should().Throw<InvalidTrackingNumberException>()
            .WithMessage("*Tracking Number cannot be empty*");
    }

    [Fact]
    public void ShipmentNumber_WithValidValue_ShouldReturnValue()
    {
        // Arrange
        const string validShipmentNumber = "SHIP123456";

        // Act
        var result = Guard.Against.ShipmentNumber(validShipmentNumber);

        // Assert
        result.Should().Be(validShipmentNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ShipmentNumber_WithInvalidValue_ShouldThrowInvalidShipmentNumberException(string invalidValue)
    {
        // Act
        var act = () => Guard.Against.ShipmentNumber(invalidValue);

        // Assert
        act.Should().Throw<InvalidShipmentNumberException>()
            .WithMessage("*Shipment number cannot be empty*");
    }

    [Fact]
    public void DpdSessionNumber_WithValidValue_ShouldReturnValue()
    {
        // Arrange
        const long validSessionNumber = 12345L;

        // Act
        var result = Guard.Against.DpdSessionNumber(validSessionNumber);

        // Assert
        result.Should().Be(validSessionNumber);
    }

    [Fact]
    public void DpdSessionNumber_WithDefaultValue_ShouldThrowInvalidSessionNumberException()
    {
        // Arrange
        const long defaultValue = 0L;

        // Act
        var act = () => Guard.Against.DpdSessionNumber(defaultValue);

        // Assert
        act.Should().Throw<InvalidSessionNumberException>()
            .WithMessage("*DPD Session number cannot be empty*");
    }

    [Fact]
    public void DpdPackageNumber_WithValidValue_ShouldReturnValue()
    {
        // Arrange
        const long validPackageNumber = 98765L;

        // Act
        var result = Guard.Against.DpdPackageNumber(validPackageNumber);

        // Assert
        result.Should().Be(validPackageNumber);
    }

    [Fact]
    public void DpdPackageNumber_WithDefaultValue_ShouldThrowInvalidPackageNumberException()
    {
        // Arrange
        const long defaultValue = 0L;

        // Act
        var act = () => Guard.Against.DpdPackageNumber(defaultValue);

        // Assert
        act.Should().Throw<InvalidPackageNumberException>()
            .WithMessage("*DPD Package number cannot be empty*");
    }

    [Fact]
    public void DpdParcelNumber_WithValidValue_ShouldReturnValue()
    {
        // Arrange
        const long validParcelNumber = 55555L;

        // Act
        var result = Guard.Against.DpdParcelNumber(validParcelNumber);

        // Assert
        result.Should().Be(validParcelNumber);
    }

    [Fact]
    public void DpdParcelNumber_WithDefaultValue_ShouldThrowInvalidPackageNumberException()
    {
        // Arrange
        const long defaultValue = 0L;

        // Act
        var act = () => Guard.Against.DpdParcelNumber(defaultValue);

        // Assert
        act.Should().Throw<InvalidPackageNumberException>()
            .WithMessage("*DPD Parcel number cannot be empty*");
    }

    [Fact]
    public void DpdWaybill_WithValidValue_ShouldReturnValue()
    {
        // Arrange
        const string validWaybill = "WB123456";

        // Act
        var result = Guard.Against.DpdWaybill(validWaybill);

        // Assert
        result.Should().Be(validWaybill);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void DpdWaybill_WithInvalidValue_ShouldThrowInvalidWaybillException(string invalidValue)
    {
        // Act
        var act = () => Guard.Against.DpdWaybill(invalidValue);

        // Assert
        act.Should().Throw<InvalidWaybillException>()
            .WithMessage("*DPD Parcel Waybill cannot be empty*");
    }
}

