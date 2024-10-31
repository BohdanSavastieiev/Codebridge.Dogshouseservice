using Codebridge.TechnicalTask.Application.Common.Constants;
using Codebridge.TechnicalTask.Application.Common.Models;
using FluentAssertions;

namespace Codebridge.TechnicalTask.Application.Tests.Tests.Common;

public class SortParametersTests
{
    [Theory]
    [InlineData("name", "asc", SortOrder.Asc)]
    [InlineData("name ", "desc", SortOrder.Desc)]
    [InlineData("name", "DESC", SortOrder.Desc)]
    [InlineData("name", null, SortOrder.Asc)]
    [InlineData(" tail_length", "", SortOrder.Asc)]
    [InlineData("tail_length", " ", SortOrder.Asc)]
    public void FactoryMethod_WithValidData_ReturnsSuccess(
        string propertyName, 
        string? order, 
        SortOrder expectedOrder)
    {
        // Act
        var result = SortParameters.Create(propertyName, order);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.PropertyName.Should().Be(propertyName.Trim());
        result.Value.Order.Should().Be(expectedOrder);
    }
    
    [Theory]
    [InlineData("", "asc")]
    [InlineData(" ", "desc")]
    [InlineData("name", "invalid")]
    [InlineData("name", " invalid")]
    [InlineData("", "")]
    [InlineData("", " ")]
    public void FactoryMethod_WithInvalidData_ReturnsFailure(
        string propertyName,
        string? order)
    {
        // Act
        var result = SortParameters.Create(propertyName, order);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}