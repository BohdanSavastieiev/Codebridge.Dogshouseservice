using System.Net;
using Codebridge.TechnicalTask.API.Common.Filters;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Codebridge.TechnicalTask.API.Tests.Tests.Common;
public record TestModel(string? NullableProperty);

public class ValidatorFilterTests
{
    private readonly Mock<IValidator<TestModel>> _validatorMock;
    private readonly ValidatorFilter<TestModel> _filter;

    public ValidatorFilterTests()
    {
        _validatorMock = new Mock<IValidator<TestModel>>();
        _filter = new ValidatorFilter<TestModel>(_validatorMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturnBadRequest_WhenParameterIsNull()
    {
        // Arrange
        var context = new DefaultEndpointFilterInvocationContext(
            new DefaultHttpContext());

        // Act
        var result = await _filter.InvokeAsync(context, _ => 
            ValueTask.FromResult<object?>(Results.Ok()));

        // Assert
        var problemResult = result.Should().BeOfType<ProblemHttpResult>().Subject;
        problemResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturnProblemDetails_WhenValidationFails()
    {
        // Arrange
        var model = new TestModel(null);
        var context = new DefaultEndpointFilterInvocationContext(
            new DefaultHttpContext(), model);

        var validationFailure = new ValidationFailure(
            "NullableProperty",
            "Property is required")
        {
            ErrorCode = "RequiredProperty"
        };

        _validatorMock
            .Setup(x => x.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([validationFailure]));

        // Act
        var result = await _filter.InvokeAsync(context, _ => 
            ValueTask.FromResult<object?>(Results.Ok()));

        // Assert
        var problemResult = result.Should().BeOfType<ProblemHttpResult>().Subject;
        problemResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallNext_WhenValidationPasses()
    {
        // Arrange
        var model = new TestModel("value");
        var context = new DefaultEndpointFilterInvocationContext(
            new DefaultHttpContext(), model);

        _validatorMock
            .Setup(x => x.ValidateAsync(model, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var nextCalled = false;

        ValueTask<object?> Next(EndpointFilterInvocationContext ctx)
        {
            nextCalled = true;
            return ValueTask.FromResult<object?>(Results.Ok());
        }

        // Act
        await _filter.InvokeAsync(context, Next);

        // Assert
        nextCalled.Should().BeTrue();
    }
}