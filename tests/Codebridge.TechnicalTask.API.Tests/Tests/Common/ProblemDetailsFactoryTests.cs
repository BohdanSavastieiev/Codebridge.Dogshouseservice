using System.Net;
using System.Text.Json;
using Codebridge.TechnicalTask.API.Common.Factories;
using Codebridge.TechnicalTask.API.Models.Common;
using Codebridge.TechnicalTask.Domain.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using static Codebridge.TechnicalTask.API.Common.Constants.ApiConstants;

namespace Codebridge.TechnicalTask.API.Tests.Tests.Common;

public class ProblemDetailsFactoryTests
{
    [Fact]
    public void CreateBadRequestProblemDetails_ShouldReturnCorrectProblemDetails()
    {
        // Arrange
        var error = Error.Validation("TEST1", "Test error");

        // Act
        var result = ProblemDetailsFactory.CreateBadRequestProblemDetails(error);

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.BadRequest, result.Status);
        Assert.Equal(HttpCodeTitles.BadRequest, result.Title);
        Assert.Equal(HttpCodeTypes.BadRequest, result.Type);
        AssertErrors(result, [error]);
    }

    [Fact]
    public void CreateNotFoundProblemDetails_ShouldReturnCorrectProblemDetails()
    {
        // Arrange
        var error = Error.NotFound("TEST2", "Test error");

        // Act
        var result = ProblemDetailsFactory.CreateNotFoundProblemDetails(error);

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.NotFound, result.Status);
        Assert.Equal(HttpCodeTitles.NotFound, result.Title);
        Assert.Equal(HttpCodeTypes.NotFound, result.Type);
        AssertErrors(result, [error]);
    }

    [Fact]
    public void CreateConflictProblemDetails_ShouldReturnCorrectProblemDetails()
    {
        // Arrange
        var error = Error.Conflict("TEST3", "Test error");

        // Act
        var result = ProblemDetailsFactory.CreateConflictProblemDetails(error);

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.Conflict, result.Status);
        Assert.Equal(HttpCodeTitles.Conflict, result.Title);
        Assert.Equal(HttpCodeTypes.Conflict, result.Type);
        AssertErrors(result, [error]);
    }

    [Fact]
    public void CreateTooManyRequestsProblemDetails_ShouldReturnCorrectProblemDetails()
    {
        // Arrange
        var error = Error.TooManyRequests("TEST4", "Test error");

        // Act
        var result = ProblemDetailsFactory.CreateTooManyRequestsProblemDetails(error);

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.TooManyRequests, result.Status);
        Assert.Equal(HttpCodeTitles.TooManyRequests, result.Title);
        Assert.Equal(HttpCodeTypes.TooManyRequests, result.Type);
        AssertErrors(result, [error]);
    }

    [Fact]
    public void CreateServiceUnavailableProblemDetails_ShouldReturnCorrectProblemDetails()
    {
        // Arrange
        var error = Error.ServiceUnavailable("TEST5", "Test error") ;

        // Act
        var result = ProblemDetailsFactory.CreateServiceUnavailableProblemDetails(error);

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.ServiceUnavailable, result.Status);
        Assert.Equal(HttpCodeTitles.ServiceUnavailable, result.Title);
        Assert.Equal(HttpCodeTypes.ServiceUnavailable, result.Type);
        AssertErrors(result, [error]);
    }

    [Fact]
    public void CreateInternalServerErrorProblemDetails_ShouldReturnCorrectProblemDetails()
    {
        // Act
        var result = ProblemDetailsFactory.CreateInternalServerErrorProblemDetails();

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.InternalServerError, result.Status);
        Assert.Equal(HttpCodeTitles.InternalServerError, result.Title);
        Assert.Equal(HttpCodeTypes.InternalServerError, result.Type);
    }
    
    private static void AssertErrors(ProblemDetails problemDetails, IEnumerable<Error> expectedErrors)
    {
        Assert.NotNull(problemDetails.Extensions);
        Assert.True(problemDetails.Extensions.ContainsKey("errors"));
        
        var errorsValue = problemDetails.Extensions["errors"];
        var errorResponses = JsonSerializer.Deserialize<ErrorResponse[]>(
            JsonSerializer.Serialize(errorsValue),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );
        
        Assert.NotNull(errorResponses);
        var expectedErrorsList = expectedErrors.ToList();
        Assert.Equal(expectedErrorsList.Count, errorResponses.Length);
        
        for (var i = 0; i < expectedErrorsList.Count; i++)
        {
            var actualError = errorResponses[i];
            var expectedError = expectedErrorsList[i];
            
            Assert.Equal(expectedError.Code, actualError.Code);
            Assert.Equal(expectedError.Message, actualError.Message);
        }
    }
}