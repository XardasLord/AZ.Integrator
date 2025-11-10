using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.Exceptions;
using AZ.Integrator.Shipments.Domain.Aggregates.InpostShipment.ValueObjects;

namespace AZ.Integrator.Shipments.Domain.Tests.Aggregates.InpostShipment.ValueObjects;

public sealed class TrackingNumberTests
{
    [Fact]
    public void Create_WithValidValue_ShouldCreateTrackingNumber()
    {
        // Arrange
        const string validNumber = "TRK123456789";

        // Act
        var trackingNumber = new TrackingNumber(validNumber);

        // Assert
        trackingNumber.Value.Should().Be(validNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidValue_ShouldThrowInvalidTrackingNumberException(string invalidNumber)
    {
        // Act
        var act = () => new TrackingNumber(invalidNumber);

        // Assert
        act.Should().Throw<InvalidTrackingNumberException>()
            .WithMessage("*Tracking Number cannot be empty*");
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string validNumber = "TRK123456789";
        var trackingNumber = new TrackingNumber(validNumber);

        // Act
        var result = trackingNumber.ToString();

        // Assert
        result.Should().Be(validNumber);
    }

    [Fact]
    public void ImplicitConversionToString_ShouldReturnValue()
    {
        // Arrange
        const string validNumber = "TRK123456789";
        var trackingNumber = new TrackingNumber(validNumber);

        // Act
        string result = trackingNumber;

        // Assert
        result.Should().Be(validNumber);
    }

    [Fact]
    public void ImplicitConversionFromString_ShouldCreateTrackingNumber()
    {
        // Arrange
        const string validNumber = "TRK123456789";

        // Act
        TrackingNumber trackingNumber = validNumber;

        // Assert
        trackingNumber.Value.Should().Be(validNumber);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        const string value = "TRK123456789";
        var trackingNumber1 = new TrackingNumber(value);
        var trackingNumber2 = new TrackingNumber(value);

        // Act
        var result = trackingNumber1.Equals(trackingNumber2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var trackingNumber1 = new TrackingNumber("TRK123456789");
        var trackingNumber2 = new TrackingNumber("TRK987654321");

        // Act
        var result = trackingNumber1.Equals(trackingNumber2);

        // Assert
        result.Should().BeFalse();
    }
}

