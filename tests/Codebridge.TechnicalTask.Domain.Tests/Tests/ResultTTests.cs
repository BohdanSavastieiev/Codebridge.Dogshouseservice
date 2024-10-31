using Codebridge.TechnicalTask.Domain.Shared.Models;
using FluentAssertions;

namespace Codebridge.TechnicalTask.Domain.Tests.Tests;

public class ResultTTests
{
    [Fact]
    public void ImplicitConversion_FromValue_CreatesSuccessResult()
    {
        // Arrange
        const string value = "test value";

        // Act
        Result<string> result = value;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Value_WhenFailure_ThrowsInvalidOperationException()
    {
        // Arrange
        var error = Error.Conflict("test.error", "Test error");
        var result = Result.Failure<string>(error);

        // Act
        var action = () => _ = result.Value;

        // Assert
        action.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void Failure_WithSingleError_CreatesFailureResult()
    {
        // Arrange
        var error = Error.Conflict("test.error", "Test error");

        // Act
        var result = Result.Failure<string>(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Should().Be(error);
    }

    [Fact]
    public void Failure_WithMultipleErrors_CreatesFailureResult()
    {
        // Arrange
        var errors = new[]
        {
            Error.Validation("error1", "First error"),
            Error.Validation("error2", "Second error")
        };

        // Act
        var result = Result.Failure<string>(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().HaveCount(2);
        result.FirstError.Should().Be(errors[0]);
    }
}