using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Models.Dogs;
using Codebridge.TechnicalTask.API.Validators.Dogs;
using FluentValidation.TestHelper;

namespace Codebridge.TechnicalTask.API.Tests.Tests.Validators;

public class CreateDogRequestValidatorTests
{
    private readonly CreateDogRequestValidator _validator;

    public CreateDogRequestValidatorTests()
    {
        _validator = new CreateDogRequestValidator();
    }
    
    private static class TestData
    {
        public static string ValidName => "Rex";
        public static string ValidColor => "Brown";
        public static int ValidTailLength => 15;
        public static int ValidWeight => 25;
    }
    
    [Fact]
    public void Validate_ShouldPass_WhenAllPropertiesAreValid()
    {
        // Arrange
        var request = new CreateDogRequest(
            Name: TestData.ValidName,
            Color: TestData.ValidColor,
            TailLength: TestData.ValidTailLength,
            Weight: TestData.ValidWeight);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_ShouldHaveError_WhenNameIsEmpty(string? name)
    {
        // Arrange
        var request = new CreateDogRequest(
            Name: name,
            Color: TestData.ValidColor,
            TailLength: TestData.ValidTailLength,
            Weight: TestData.ValidWeight);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorCode(ApiErrorCodes.InvalidRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_ShouldHaveError_WhenColorIsEmpty(string? color)
    {
        // Arrange
        var request = new CreateDogRequest(
            Name: TestData.ValidName,
            Color: color,
            TailLength: TestData.ValidTailLength,
            Weight: TestData.ValidWeight);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Color)
            .WithErrorCode(ApiErrorCodes.InvalidRequest);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenTailLengthIsNull()
    {
        // Arrange
        var request = new CreateDogRequest(
            Name: TestData.ValidName,
            Color: TestData.ValidColor,
            TailLength: null,
            Weight: TestData.ValidWeight);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TailLength)
            .WithErrorCode(ApiErrorCodes.InvalidRequest);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenWeightIsNull()
    {
        // Arrange
        var request = new CreateDogRequest(
            Name: TestData.ValidName,
            Color: TestData.ValidColor,
            TailLength: TestData.ValidTailLength,
            Weight: null);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Weight)
            .WithErrorCode(ApiErrorCodes.InvalidRequest);
    }

    [Fact]
    public void Validate_ShouldHaveMultipleErrors_WhenAllPropertiesAreInvalid()
    {
        // Arrange
        var request = new CreateDogRequest(
            Name: "",
            Color: "",
            TailLength: null,
            Weight: null);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorCode(ApiErrorCodes.InvalidRequest);
        result.ShouldHaveValidationErrorFor(x => x.Color)
            .WithErrorCode(ApiErrorCodes.InvalidRequest);
        result.ShouldHaveValidationErrorFor(x => x.TailLength)
            .WithErrorCode(ApiErrorCodes.InvalidRequest);
        result.ShouldHaveValidationErrorFor(x => x.Weight)
            .WithErrorCode(ApiErrorCodes.InvalidRequest);
    }
}