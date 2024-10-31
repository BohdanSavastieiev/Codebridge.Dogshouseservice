using Codebridge.TechnicalTask.Application.Common.Constants;
using Codebridge.TechnicalTask.Application.Common.Extensions;
using FluentAssertions;

namespace Codebridge.TechnicalTask.Application.Tests.Tests.Common;

public class SortOrderExtensionsTests
{
    [Theory]
    [InlineData("asc")]
    [InlineData("desc")]
    [InlineData("ASC")]
    [InlineData("DESC")]
    public void IsValid_ShouldReturnTrue_WhenOrderIsValid(string order)
    {
        // Act
        var result = SortOrderExtensions.IsValid(order);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid")]
    [InlineData("ascending")]
    public void IsValid_ShouldReturnFalse_WhenOrderIsInvalid(string order)
    {
        // Act
        var result = SortOrderExtensions.IsValid(order);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("asc", SortOrder.Asc)]
    [InlineData("ASC", SortOrder.Asc)]
    [InlineData("desc", SortOrder.Desc)]
    [InlineData("DESC", SortOrder.Desc)]
    public void TryParse_ShouldReturnTrueAndSetValue_WhenOrderIsValid(string order, SortOrder expectedOrder)
    {
        // Act
        var success = SortOrderExtensions.TryParse(order, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().Be(expectedOrder);
    }

    [Theory]
    
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid")]
    public void TryParse_ShouldReturnFalseAndSetNull_WhenOrderIsInvalid(string order)
    {
        // Act
        var success = SortOrderExtensions.TryParse(order, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }
}