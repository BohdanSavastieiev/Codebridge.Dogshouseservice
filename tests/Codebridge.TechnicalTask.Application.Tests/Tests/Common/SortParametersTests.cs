using Codebridge.TechnicalTask.Application.Common.Constants;
using Codebridge.TechnicalTask.Application.Common.Models;
using FluentAssertions;

namespace Codebridge.TechnicalTask.Application.Tests.Tests.Common;

public class SortParametersTests
{
    [Theory]
    [InlineData("name", "asc", SortOrder.Asc)]
    [InlineData("name", "desc", SortOrder.Desc)]
    [InlineData("name", "DESC", SortOrder.Desc)]
    [InlineData("name", null, SortOrder.Asc)]
    public void Constructor_InitializesCorrectly(
        string propertyName, 
        string? order, 
        SortOrder expectedOrder)
    {
        // Act
        var parameters = new SortParameters(propertyName, order);

        // Assert
        parameters.PropertyName.Should().Be(propertyName);
        parameters.Order.Should().Be(expectedOrder);
    }
    
    [Theory]
    [InlineData("name", "")]
    [InlineData("name", "invalid")]
    public void Constructor_ShouldThrowException_WhenStringSortOrderIsNotNullAndInvalid(
        string propertyName,
        string? order)
    {
        // Act
        var action = () => new SortParameters(propertyName, order);

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }
}