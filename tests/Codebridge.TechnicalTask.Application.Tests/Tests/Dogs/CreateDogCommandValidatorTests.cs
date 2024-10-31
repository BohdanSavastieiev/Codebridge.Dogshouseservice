using Codebridge.TechnicalTask.Application.Dogs.Commands.CreateDog;
using Codebridge.TechnicalTask.Domain.Common.Constants;
using FluentValidation.TestHelper;

namespace Codebridge.TechnicalTask.Application.Tests.Tests.Dogs;

public class CreateDogCommandValidatorTests
{
    private readonly CreateDogCommandValidator _validator;
    private static readonly TestData ValidData = new();

    public CreateDogCommandValidatorTests()
    {
        _validator = new CreateDogCommandValidator();
    }

    public static IEnumerable<object[]> ValidNames =>
    [
        ["A"],
        ["ValidName"],
        [new string('a', DomainConstants.Dog.MaxNameLength)]
    ];

    public static IEnumerable<object[]> ValidColors =>
    [
        ["Black"],
        ["Black&Yellow"],
        [new string('a', DomainConstants.Dog.MaxColorLength)]
    ];

    private class TestData
    {
        public string ValidName => "Rex";
        public string ValidColor => "Brown";
        public double ValidTailLength => 10.0;
        public double ValidWeight => 20.0;
    }

    [Theory]
    [MemberData(nameof(ValidNames))]
    public void ValidateName_ShouldPassForValidLength(string name)
    {
        // Arrange
        var command = new CreateDogCommand(
            name,
            ValidData.ValidColor,
            ValidData.ValidTailLength,
            ValidData.ValidWeight);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
    
    [Theory]
    [MemberData(nameof(ValidColors))]
    public void ValidateColor_ShouldPassForValidLength(string color)
    {
        // Arrange
        var command = new CreateDogCommand(
            ValidData.ValidName,
            color,
            ValidData.ValidTailLength,
            ValidData.ValidWeight);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ValidateName_ShouldFailForTooLongName()
    {
        // Arrange
        var command = new CreateDogCommand(
            new string('a', DomainConstants.Dog.MaxNameLength + 1),
            ValidData.ValidColor,
            ValidData.ValidTailLength,
            ValidData.ValidWeight);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorCode(DomainErrorCodes.Dog.Validation.InvalidNameLength);
    }
    
    [Fact]
    public void ValidateColor_ShouldFailForTooLongColor()
    {
        // Arrange
        var command = new CreateDogCommand(
            ValidData.ValidName,
            new string('a', DomainConstants.Dog.MaxColorLength + 1),
            ValidData.ValidTailLength,
            ValidData.ValidWeight);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Color)
            .WithErrorCode(DomainErrorCodes.Dog.Validation.InvalidColorLength);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.1)]
    [InlineData(-1)]    
    public void ValidateWeight_ShouldFailForInvalidWeight(double weight)
    {
        // Arrange
        var command = new CreateDogCommand(
            ValidData.ValidName,
            ValidData.ValidColor,
            ValidData.ValidTailLength,
            weight);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Weight)
            .WithErrorCode(DomainErrorCodes.Dog.Validation.InvalidWeight);
    }

    [Theory] 
    [InlineData(-0.1)]
    [InlineData(-1)]
    public void ValidateTailLength_ShouldFailForNegativeLength(double tailLength)
    {
        // Arrange
        var command = new CreateDogCommand(
            ValidData.ValidName,
            ValidData.ValidColor,
            tailLength,
            ValidData.ValidWeight);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TailLength)
            .WithErrorCode(DomainErrorCodes.Dog.Validation.InvalidTailLength);
    }
}