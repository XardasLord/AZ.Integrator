using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.Exceptions;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.DpdShipment.Exceptions;

public sealed class InvalidSessionNumberExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const long sessionNumber = 12345L;
        const string message = "Invalid session number";

        // Act
        var exception = new InvalidSessionNumberException(sessionNumber, message);

        // Assert
        exception.SessionNumber.Should().Be(sessionNumber);
        exception.Message.Should().Be(message);
        exception.Code.Should().Be("invalid_session_number");
    }

    [Fact]
    public void Code_ShouldReturnCorrectErrorCode()
    {
        // Arrange
        var exception = new InvalidSessionNumberException(12345L, "Error message");

        // Act
        var code = exception.Code;

        // Assert
        code.Should().Be("invalid_session_number");
    }
}

