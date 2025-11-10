using AZ.Integrator.Procurement.Domain.Aggregates.PartDefinitionsOrder.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Tests.Aggregates.PartDefinitionsOrder.ValueObjects;

public sealed class DimensionsTests
{
    [Fact]
    public void Create_WithValidValues_ShouldCreateDimensions()
    {
        // Arrange
        const int lengthMm = 1000;
        const int widthMm = 500;
        const int thicknessMm = 18;
        var lengthEdgeBanding = EdgeBandingType.One;
        var widthEdgeBanding = EdgeBandingType.Two;

        // Act
        var result = Act(() => Dimensions.Create(lengthMm, widthMm, thicknessMm, lengthEdgeBanding, widthEdgeBanding));

        // Assert
        result.LengthMm.Should().Be(lengthMm);
        result.WidthMm.Should().Be(widthMm);
        result.ThicknessMm.Should().Be(thicknessMm);
        result.LengthEdgeBandingType.Should().Be(lengthEdgeBanding);
        result.WidthEdgeBandingType.Should().Be(widthEdgeBanding);
    }

    [Theory]
    [InlineData(0, 500, 18)]
    [InlineData(-1, 500, 18)]
    public void Create_WithInvalidLength_ShouldThrowArgumentException(int invalidLength, int width, int thickness)
    {
        // Act
        var act = () => Act(() => Dimensions.Create(invalidLength, width, thickness, EdgeBandingType.None, EdgeBandingType.None));

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Length must be greater than 0*");
    }

    [Theory]
    [InlineData(1000, 0, 18)]
    [InlineData(1000, -1, 18)]
    public void Create_WithInvalidWidth_ShouldThrowArgumentException(int length, int invalidWidth, int thickness)
    {
        // Act
        var act = () => Act(() => Dimensions.Create(length, invalidWidth, thickness, EdgeBandingType.None, EdgeBandingType.None));

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Width must be greater than 0*");
    }

    [Theory]
    [InlineData(1000, 500, 0)]
    [InlineData(1000, 500, -1)]
    public void Create_WithInvalidThickness_ShouldThrowArgumentException(int length, int width, int invalidThickness)
    {
        // Act
        var act = () => Act(() => Dimensions.Create(length, width, invalidThickness, EdgeBandingType.None, EdgeBandingType.None));

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Thickness must be greater than 0*");
    }

    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var dimensions1 = Dimensions.Create(1000, 500, 18, EdgeBandingType.One, EdgeBandingType.Two);
        var dimensions2 = Dimensions.Create(1000, 500, 18, EdgeBandingType.One, EdgeBandingType.Two);

        // Act
        var result = Act(() => dimensions1.Equals(dimensions2));

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var dimensions1 = Dimensions.Create(1000, 500, 18, EdgeBandingType.One, EdgeBandingType.Two);
        var dimensions2 = Dimensions.Create(2000, 600, 18, EdgeBandingType.None, EdgeBandingType.One);

        // Act
        var result = Act(() => dimensions1.Equals(dimensions2));

        // Assert
        result.Should().BeFalse();
    }

    private static T Act<T>(Func<T> action) => action();
    private static void Act(Action action) => action();
}

