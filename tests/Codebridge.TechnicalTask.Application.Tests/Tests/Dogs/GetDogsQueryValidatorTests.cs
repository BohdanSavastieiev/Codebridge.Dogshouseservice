using Codebridge.TechnicalTask.Application.Common.Models;
using Codebridge.TechnicalTask.Application.Dogs.Queries.GetDogs;
using Codebridge.TechnicalTask.Domain.Common.Constants;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Codebridge.TechnicalTask.Application.Tests.Tests.Dogs;

public class GetDogsQueryValidatorTests
{
    private readonly GetDogsQueryValidator _validator;

    public GetDogsQueryValidatorTests()
    {
        _validator = new GetDogsQueryValidator();
    }

    [Theory]
    [InlineData("name")]
    [InlineData("tail_length")]
    [InlineData("weight")]
    [InlineData("color")]
    public void Validate_ShouldPassForValidSortProperties(string propertyName)
    {
        // Arrange
        var paginationParams = new PaginationParameters();
        var sortParams = new SortParameters(propertyName);
        var query = new GetDogsQuery(paginationParams, sortParams);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.SortParameters!.PropertyName);
    }

    [Theory]
    [InlineData("invalid_property")]
    [InlineData("")]
    public void Validate_ShouldFailForInvalidSortProperties(string propertyName)
    {
        // Arrange
        var paginationParams = new PaginationParameters();
        var sortParams = new SortParameters(propertyName);
        var query = new GetDogsQuery(paginationParams, sortParams);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SortParameters!.PropertyName)
            .WithErrorCode(DomainErrorCodes.Dog.Validation.InvalidSortProperty);
    }

    [Fact]
    public void Validate_ShouldPassWhenSortParametersAreNull()
    {
        // Arrange
        var paginationParams = new PaginationParameters();
        var query = new GetDogsQuery(paginationParams);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}