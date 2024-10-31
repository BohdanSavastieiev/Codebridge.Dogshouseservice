using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Models.Common;
using Codebridge.TechnicalTask.API.Validators.Common;
using Codebridge.TechnicalTask.Application.Common.Constants;
using FluentValidation.TestHelper;

namespace Codebridge.TechnicalTask.API.Tests.Tests.Validators;

public class PaginationRequestValidatorTests
{
    private readonly PaginationRequestValidator _validator;

    public PaginationRequestValidatorTests()
    {
        _validator = new PaginationRequestValidator();
    }
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(100, 50)]
    [InlineData(1, int.MaxValue)]
    [InlineData(int.MaxValue, 1)]
    public void Validate_ShouldPass_WhenPropertiesAreValid(int pageNumber, int pageSize)
    {
        // Arrange
        var request = new PaginationRequest(pageNumber, pageSize);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_ShouldHaveError_WhenPageSizeIsInvalid(int pageSize)
    {
        // Arrange
        var request = new PaginationRequest(pageSize, null);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorCode(ApplicationErrorCodes.Pagination.InvalidPageSize);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_ShouldHaveError_WhenPageNumberIsInvalid(int pageNumber)
    {
        // Arrange
        var request = new PaginationRequest(null, pageNumber);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber)
            .WithErrorCode(ApplicationErrorCodes.Pagination.InvalidPageNumber);
    }

    [Fact]
    public void Validate_ShouldPass_WhenPropertiesAreNull()
    {
        // Arrange
        var request = new PaginationRequest(null, null);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}