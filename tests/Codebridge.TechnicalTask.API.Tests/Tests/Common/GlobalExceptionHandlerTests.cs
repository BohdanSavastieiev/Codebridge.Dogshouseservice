using System.Text.Json;
using Codebridge.TechnicalTask.API.Common.Exceptions;
using Codebridge.TechnicalTask.API.Common.Middlewares;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Codebridge.TechnicalTask.API.Tests.Tests.Common;

public class GlobalExceptionHandlerTests
{
    private readonly GlobalExceptionHandler _handler;
    private readonly DefaultHttpContext _httpContext;
    private readonly MemoryStream _responseBody;

    public GlobalExceptionHandlerTests()
    {
        Mock<ILogger<GlobalExceptionHandler>> loggerMock = new();
        _handler = new GlobalExceptionHandler(loggerMock.Object);
        
        _responseBody = new MemoryStream();
        _httpContext = new DefaultHttpContext
        {
            Response =
            {
                Body = _responseBody
            },
            TraceIdentifier = "test-trace-id"
        };
    }

    [Fact]
    public async Task TryHandleAsync_WithBadHttpRequestException_ReturnsBadRequestProblemDetails()
    {
        // Arrange
        var exception = new BadHttpRequestException("Invalid request");

        // Act
        var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);
        _responseBody.Position = 0;
        var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(_responseBody);

        // Assert
        result.Should().BeTrue();
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task TryHandleAsync_WithApiConfigurationException_ReturnsServiceUnavailableProblemDetails()
    {
        // Arrange
        var exception = new ApiConfigurationException("Configuration missing");

        // Act
        var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);
        _responseBody.Position = 0;
        var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(_responseBody);

        // Assert
        result.Should().BeTrue();
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status503ServiceUnavailable);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status503ServiceUnavailable);
    }

    [Fact]
    public async Task TryHandleAsync_WithUnhandledException_ReturnsInternalServerErrorProblemDetails()
    {
        // Arrange
        var exception = new Exception("Unexpected error");

        // Act
        var result = await _handler.TryHandleAsync(_httpContext, exception, CancellationToken.None);
        _responseBody.Position = 0;
        var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(_responseBody);

        // Assert
        result.Should().BeTrue();
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(StatusCodes.Status500InternalServerError);
    }
}