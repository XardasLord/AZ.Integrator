using AZ.Integrator.Shipments.Domain.Aggregates.DpdShipment.Exceptions;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.DpdShipment.Exceptions;

public sealed class InvalidPackageNumberExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        const long packageNumber = 98765L;
        const string message = "Invalid package number";

        // Act
        var exception = new InvalidPackageNumberException(packageNumber, message);

        // Assert
        exception.PackageNumber.Should().Be(packageNumber);
        exception.Message.Should().Be(message);
        exception.Code.Should().Be("invalid_package_number");
    }

    [Fact]
    public void Code_ShouldReturnCorrectErrorCode()
    {
        // Arrange
        var exception = new InvalidPackageNumberException(98765L, "Error message");

        // Act
        var code = exception.Code;

        // Assert
        code.Should().Be("invalid_package_number");
    }
}

