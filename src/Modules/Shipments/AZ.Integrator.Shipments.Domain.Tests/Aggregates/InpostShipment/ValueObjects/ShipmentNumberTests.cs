using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.InpostShipment.ValueObjects;

public sealed class ShipmentNumberTests
{
    [Fact]
    public void Create_WithValidValue_ShouldCreateShipmentNumber()
    {
        // Arrange
        const string validNumber = "SHIP123456";

        // Act
        var shipmentNumber = new ShipmentNumber(validNumber);

        // Assert
        shipmentNumber.Value.Should().Be(validNumber);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string validNumber = "SHIP123456";
        var shipmentNumber = new ShipmentNumber(validNumber);

        // Act
        var result = shipmentNumber.ToString();

        // Assert
        result.Should().Be(validNumber);
    }

    [Fact]
    public void ImplicitConversionToString_ShouldReturnValue()
    {
        // Arrange
        const string validNumber = "SHIP123456";
        var shipmentNumber = new ShipmentNumber(validNumber);

        // Act
        string result = shipmentNumber;

        // Assert
        result.Should().Be(validNumber);
    }

    [Fact]
    public void ImplicitConversionFromString_ShouldCreateShipmentNumber()
    {
        // Arrange
        const string validNumber = "SHIP123456";

        // Act
        ShipmentNumber shipmentNumber = validNumber;

        // Assert
        shipmentNumber.Value.Should().Be(validNumber);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        const string value = "SHIP123456";
        var shipmentNumber1 = new ShipmentNumber(value);
        var shipmentNumber2 = new ShipmentNumber(value);

        // Act
        var result = shipmentNumber1.Equals(shipmentNumber2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var shipmentNumber1 = new ShipmentNumber("SHIP123456");
        var shipmentNumber2 = new ShipmentNumber("SHIP654321");

        // Act
        var result = shipmentNumber1.Equals(shipmentNumber2);

        // Assert
        result.Should().BeFalse();
    }
}

