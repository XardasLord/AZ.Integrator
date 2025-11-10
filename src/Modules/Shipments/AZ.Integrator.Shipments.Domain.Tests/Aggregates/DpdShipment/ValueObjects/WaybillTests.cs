using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.DpdShipment.ValueObjects;

public sealed class WaybillTests
{
    [Fact]
    public void Create_WithValidValue_ShouldCreateWaybill()
    {
        // Arrange
        const string validWaybill = "WB123456789";

        // Act
        var waybill = new Waybill(validWaybill);

        // Assert
        waybill.Value.Should().Be(validWaybill);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string validWaybill = "WB123456789";
        var waybill = new Waybill(validWaybill);

        // Act
        var result = waybill.ToString();

        // Assert
        result.Should().Be(validWaybill);
    }

    [Fact]
    public void ImplicitConversionToString_ShouldReturnValue()
    {
        // Arrange
        const string validWaybill = "WB123456789";
        var waybill = new Waybill(validWaybill);

        // Act
        string result = waybill;

        // Assert
        result.Should().Be(validWaybill);
    }

    [Fact]
    public void ImplicitConversionFromString_ShouldCreateWaybill()
    {
        // Arrange
        const string validWaybill = "WB123456789";

        // Act
        Waybill waybill = validWaybill;

        // Assert
        waybill.Value.Should().Be(validWaybill);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        const string value = "WB123456789";
        var waybill1 = new Waybill(value);
        var waybill2 = new Waybill(value);

        // Act
        var result = waybill1.Equals(waybill2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var waybill1 = new Waybill("WB123456789");
        var waybill2 = new Waybill("WB987654321");

        // Act
        var result = waybill1.Equals(waybill2);

        // Assert
        result.Should().BeFalse();
    }
}

