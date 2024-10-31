using Codebridge.TechnicalTask.Application.Common.Models;
using FluentAssertions;
using static Codebridge.TechnicalTask.Application.Common.Constants.ApplicationConstants.Pagination;

namespace Codebridge.TechnicalTask.Application.Tests.Tests.Common;

public class PaginationParametersTests
{
    [Fact]
    public void EmptyConstructor_InitializesCorrectly()
    {
        // Act
        var parameters = new PaginationParameters();

        // Assert
        parameters.PageSize.Should().Be(DefaultPageSize);
        parameters.PageNumber.Should().Be(DefaultPageNumber);
    }
    
    [Theory]
    [InlineData(5, 1, 5, 1)]
    [InlineData(null, null, DefaultPageSize, DefaultPageNumber)]
    [InlineData(300, 1, MaxPageSize, 1)]
    [InlineData(-1, 1, DefaultPageSize, 1)]
    [InlineData(10, -1, 10, DefaultPageNumber)]
    public void ConstructorWithParameters_InitializesCorrectly(
        int? pageSize, 
        int? pageNumber,
        int expectedSize,
        int expectedNumber)
    {
        // Act
        var parameters = new PaginationParameters(pageSize, pageNumber);

        // Assert
        parameters.PageSize.Should().Be(expectedSize);
        parameters.PageNumber.Should().Be(expectedNumber);
    }
}