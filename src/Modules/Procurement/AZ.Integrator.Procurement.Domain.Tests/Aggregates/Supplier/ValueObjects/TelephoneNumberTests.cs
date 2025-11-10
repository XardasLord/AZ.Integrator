using AZ.Integrator.Procurement.Domain.Aggregates.Supplier.ValueObjects;

namespace AZ.Integrator.Procurement.Domain.Tests.Aggregates.Supplier.ValueObjects;

public sealed class TelephoneNumberTests
{
    [Fact]
    public void Create_WithValidValue_ShouldCreateTelephoneNumber()
    {
        // Arrange
        const string validNumber = "+48 123 456 789";

        // Act
        var result = Act(() => TelephoneNumber.Create(validNumber));

        // Assert
        result.Value.Should().Be(validNumber);
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrimValue()
    {
        // Arrange
        const string numberWithWhitespace = "  +48 123 456 789  ";
        const string expectedNumber = "+48 123 456 789";

        // Act
        var result = Act(() => TelephoneNumber.Create(numberWithWhitespace));

        // Assert
        result.Value.Should().Be(expectedNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyValue_ShouldReturnEmptyString(string emptyValue)
    {
        // Act
        var result = Act(() => TelephoneNumber.Create(emptyValue));

        // Assert
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        const string validNumber = "+48 123 456 789";
        var telephoneNumber = TelephoneNumber.Create(validNumber);

        // Act
        var result = Act(() => telephoneNumber.ToString());

        // Assert
        result.Should().Be(validNumber);
    }

    [Fact]
    public void ImplicitConversionToString_ShouldReturnValue()
    {
        // Arrange
        const string validNumber = "+48 123 456 789";
        var telephoneNumber = TelephoneNumber.Create(validNumber);

        // Act
        string result = Act(() => (string)telephoneNumber);

        // Assert
        result.Should().Be(validNumber);
    }

    [Fact]
    public void ImplicitConversionFromString_ShouldCreateTelephoneNumber()
    {
        // Arrange
        const string validNumber = "+48 123 456 789";

        // Act
        TelephoneNumber result = Act(() => (TelephoneNumber)validNumber);

        // Assert
        result.Value.Should().Be(validNumber);
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        const string value = "+48 123 456 789";
        var telephoneNumber1 = TelephoneNumber.Create(value);
        var telephoneNumber2 = TelephoneNumber.Create(value);

        // Act
        var result = Act(() => telephoneNumber1.Equals(telephoneNumber2));

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var telephoneNumber1 = TelephoneNumber.Create("+48 123 456 789");
        var telephoneNumber2 = TelephoneNumber.Create("+48 987 654 321");

        // Act
        var result = Act(() => telephoneNumber1.Equals(telephoneNumber2));

        // Assert
        result.Should().BeFalse();
    }

    private static T Act<T>(Func<T> action) => action();
    private static void Act(Action action) => action();
}

