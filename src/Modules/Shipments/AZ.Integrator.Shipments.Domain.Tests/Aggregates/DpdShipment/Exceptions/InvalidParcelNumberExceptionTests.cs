using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.Exceptions;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.DpdShipment.Exceptions;

public sealed class InvalidParcelNumberExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const long parcelNumber = 55555L;
        const string message = "Invalid parcel number";

        // Act
        var exception = new InvalidParcelNumberException(parcelNumber, message);

        // Assert
        exception.ParcelNumber.Should().Be(parcelNumber);
        exception.Message.Should().Be(message);
        exception.Code.Should().Be("invalid_parcel_number");
    }

    [Fact]
    public void Code_ShouldReturnCorrectErrorCode()
    {
        // Arrange
        var exception = new InvalidParcelNumberException(55555L, "Error message");

        // Act
        var code = exception.Code;

        // Assert
        code.Should().Be("invalid_parcel_number");
    }
}

