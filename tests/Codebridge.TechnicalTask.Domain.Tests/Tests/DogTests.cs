using Codebridge.TechnicalTask.Domain.Common.Constants;
using Codebridge.TechnicalTask.Domain.Common.Exceptions;
using Codebridge.TechnicalTask.Domain.Dogs.Entities;
using FluentAssertions;

namespace Codebridge.TechnicalTask.Domain.Tests.Tests;

public class DogTests
{
    private static class ValidDogData
    {
        public const string Name = "Buddy";
        public const string Color = "Brown";
        public const double TailLength = 15.5;
        public const double Weight = 25.0;
    }

    [Fact]
    public void Constructor_WithValidData_CreatesInstance()
    {
        // Arrange and Act
        var dog = new Dog(ValidDogData.Name, ValidDogData.Color, 
            ValidDogData.TailLength, ValidDogData.Weight);

        // Assert
        dog.Name.Should().Be(ValidDogData.Name);
        dog.Color.Should().Be(ValidDogData.Color);
        dog.TailLength.Should().Be(ValidDogData.TailLength);
        dog.Weight.Should().Be(ValidDogData.Weight);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WithInvalidName_ThrowsInvalidDomainException(string invalidName)
    {
        // Arrange & Act
        var action = () => new Dog(invalidName, ValidDogData.Color, 
            ValidDogData.TailLength, ValidDogData.Weight);

        // Assert
        action.Should().ThrowExactly<InvalidDomainException>();
    }

    [Fact]
    public void Constructor_WithTooLongName_ThrowsInvalidDomainException()
    {
        // Arrange
        var tooLongName = new string('x', DomainConstants.Dog.MaxNameLength + 1);

        // Act
        var action = () => new Dog(tooLongName, ValidDogData.Color, 
            ValidDogData.TailLength, ValidDogData.Weight);

        // Assert
        action.Should().ThrowExactly<InvalidDomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_WithInvalidColor_ThrowsInvalidDomainException(string invalidColor)
    {
        // Arrange & Act
        var action = () => new Dog(ValidDogData.Name, invalidColor, 
            ValidDogData.TailLength, ValidDogData.Weight);

        // Assert
        action.Should().ThrowExactly<InvalidDomainException>();
    }

    [Fact]
    public void Constructor_WithTooLongColor_ThrowsInvalidDomainException()
    {
        // Arrange
        var tooLongColor = new string('x', DomainConstants.Dog.MaxColorLength + 1);

        // Act
        var action = () => new Dog(ValidDogData.Name, tooLongColor, 
            ValidDogData.TailLength, ValidDogData.Weight);

        // Assert
        action.Should().ThrowExactly<InvalidDomainException>();
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-0.1)]
    public void Constructor_WithNegativeTailLength_ThrowsInvalidDomainException(double invalidTailLength)
    {
        // Act
        var action = () => new Dog(ValidDogData.Name, ValidDogData.Color, 
            invalidTailLength, ValidDogData.Weight);

        // Assert
        action.Should().ThrowExactly<InvalidDomainException>();
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    public void Constructor_WithInvalidWeight_ThrowsInvalidDomainException(double invalidWeight)
    {
        // Act
        var action = () => new Dog(ValidDogData.Name, ValidDogData.Color, 
            ValidDogData.TailLength, invalidWeight);

        // Assert
        action.Should().ThrowExactly<InvalidDomainException>();
    }

    [Fact]
    public void Constructor_WithMaxLengthName_CreatesInstance()
    {
        // Arrange
        var maxLengthName = new string('x', DomainConstants.Dog.MaxNameLength);

        // Act
        var dog = new Dog(maxLengthName, ValidDogData.Color, 
            ValidDogData.TailLength, ValidDogData.Weight);

        // Assert
        dog.Name.Should().Be(maxLengthName);
    }

    [Fact]
    public void Constructor_WithMaxLengthColor_CreatesInstance()
    {
        // Arrange
        var maxLengthColor = new string('x', DomainConstants.Dog.MaxColorLength);

        // Act
        var dog = new Dog(ValidDogData.Name, maxLengthColor, 
            ValidDogData.TailLength, ValidDogData.Weight);

        // Assert
        dog.Color.Should().Be(maxLengthColor);
    }
}