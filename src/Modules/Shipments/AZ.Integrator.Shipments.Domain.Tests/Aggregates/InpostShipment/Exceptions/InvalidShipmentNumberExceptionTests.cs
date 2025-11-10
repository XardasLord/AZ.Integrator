using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Exceptions;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.InpostShipment.Exceptions;

public sealed class InvalidShipmentNumberExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const string shipmentNumber = "SHIP123";
        const string message = "Invalid shipment number";

        // Act
        var exception = new InvalidShipmentNumberException(shipmentNumber, message);

        // Assert
        exception.ShipmentNumber.Should().Be(shipmentNumber);
        exception.Message.Should().Be(message);
        exception.Code.Should().Be("invalid_shipment_number");
    }

    [Fact]
    public void Code_ShouldReturnCorrectErrorCode()
    {
        // Arrange
        var exception = new InvalidShipmentNumberException("SHIP123", "Error message");

        // Act
        var code = exception.Code;

        // Assert
        code.Should().Be("invalid_shipment_number");
    }
}

