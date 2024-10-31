using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.API.Models.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using static Codebridge.TechnicalTask.Application.Common.Constants.ApplicationConstants.Pagination;

namespace Codebridge.TechnicalTask.API.Tests.Tests.Common;

public class PaginationHeaderResultTests
{
    private readonly Mock<IResult> _resultMock;
    private readonly DefaultHttpContext _httpContext;
    private readonly PaginationMetadata _metadata;

    public PaginationHeaderResultTests()
    {
        _resultMock = new Mock<IResult>();
        _httpContext = new DefaultHttpContext();
        _metadata = new PaginationMetadata(DefaultPageNumber, DefaultPageSize, 0, 0);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSetAllPaginationHeaders()
    {
        // Arrange
        var result = new PaginationHeaderResult(_resultMock.Object, _metadata);

        // Act
        await result.ExecuteAsync(_httpContext);

        // Assert
        _httpContext.Response.Headers[ApiConstants.HttpHeaders.PageNumber].ToString()
            .Should().Be(DefaultPageNumber.ToString());
        _httpContext.Response.Headers[ApiConstants.HttpHeaders.PageSize].ToString()
            .Should().Be(DefaultPageSize.ToString());
        _httpContext.Response.Headers[ApiConstants.HttpHeaders.TotalPages].ToString()
            .Should().Be("0");
        _httpContext.Response.Headers[ApiConstants.HttpHeaders.TotalCount].ToString()
            .Should().Be("0");
        
        _resultMock.Verify(x => x.ExecuteAsync(_httpContext), Times.Once);
    }
}