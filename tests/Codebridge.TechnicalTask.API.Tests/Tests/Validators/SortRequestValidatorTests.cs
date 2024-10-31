using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Models.Common;
using Codebridge.TechnicalTask.API.Validators.Common;
using FluentValidation.TestHelper;

namespace Codebridge.TechnicalTask.API.Tests.Tests.Validators;

public class SortRequestValidatorTests
{
    private readonly SortRequestValidator _validator;

    public SortRequestValidatorTests()
    {
        _validator = new SortRequestValidator();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("name", null)]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("name", "ASC")]
    [InlineData("name", "DESC")]
    public void Validate_ShouldPass_WhenValuesAreValid(string? attribute, string? order)
    {
        // Arrange
        var request = new SortRequest(attribute, order);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null, "asc")]
    [InlineData("", "desc")]
    [InlineData(" ", "asc")]
    public void Validate_ShouldHaveError_WhenAttributeIsEmptyWithOrder(string? attribute, string order)
    {
        // Arrange
        var request = new SortRequest(attribute, order);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Attribute)
            .WithErrorCode(ApiErrorCodes.Sort.AttributeRequired);
    }

    [Theory]
    [InlineData("random")]
    [InlineData("ascending")]
    [InlineData("descending")]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_ShouldHaveError_WhenOrderIsInvalid(string order)
    {
        // Arrange
        var request = new SortRequest("name", order);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Order)
            .WithErrorCode(ApiErrorCodes.Sort.InvalidOrder);
    }
}