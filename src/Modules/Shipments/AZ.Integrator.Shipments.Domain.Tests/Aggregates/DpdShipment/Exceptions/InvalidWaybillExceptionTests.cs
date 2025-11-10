using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.Exceptions;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.DpdShipment.Exceptions;

public sealed class InvalidWaybillExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const string waybill = "WB123";
        const string message = "Invalid waybill";

        // Act
        var exception = new InvalidWaybillException(waybill, message);

        // Assert
        exception.Waybill.Should().Be(waybill);
        exception.Message.Should().Be(message);
        exception.Code.Should().Be("invalid_waybill");
    }

    [Fact]
    public void Code_ShouldReturnCorrectErrorCode()
    {
        // Arrange
        var exception = new InvalidWaybillException("WB123", "Error message");

        // Act
        var code = exception.Code;

        // Assert
        code.Should().Be("invalid_waybill");
    }
}

