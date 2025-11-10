using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Exceptions;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.InpostShipment.Exceptions;

public sealed class InvalidTrackingNumberExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const string trackingNumber = "TRK123";
        const string message = "Invalid tracking number";

        // Act
        var exception = new InvalidTrackingNumberException(trackingNumber, message);

        // Assert
        exception.TrackingNumber.Should().Be(trackingNumber);
        exception.Message.Should().Be(message);
        exception.Code.Should().Be("invalid_tracking_number");
    }

    [Fact]
    public void Code_ShouldReturnCorrectErrorCode()
    {
        // Arrange
        var exception = new InvalidTrackingNumberException("TRK123", "Error message");

        // Act
        var code = exception.Code;

        // Assert
        code.Should().Be("invalid_tracking_number");
    }
}

