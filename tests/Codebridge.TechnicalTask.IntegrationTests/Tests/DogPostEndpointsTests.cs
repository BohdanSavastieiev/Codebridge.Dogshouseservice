using System.Net;
using System.Net.Http.Json;
using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.Application.Dogs.Common;
using Codebridge.TechnicalTask.Infrastructure.Persistence.Seed;
using Codebridge.TechnicalTask.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Codebridge.TechnicalTask.IntegrationTests.Tests;

public class DogPostEndpointsTests : BaseIntegrationTest
{
    private const string BaseUrl = $"{ApiConstants.ApiVersionedPath}/dogs";
    
    public DogPostEndpointsTests(TestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateDog_WithValidData_ReturnsCreated()
    {
        var uniqueName = $"TestDog_{Guid.NewGuid()}";
        var createDogRequest = new
        {
            name = uniqueName,
            color = "Brown",
            tail_length = 15,
            weight = 25
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDogRequest, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        
        var createdDog = await response.Content.ReadFromJsonAsync<DogDto>(JsonOptions);
        createdDog.Should().NotBeNull();
        createdDog!.Name.Should().Be(uniqueName);
    }
    
    [Theory]
    [InlineData("", "Brown", 15, 25)]
    [InlineData("Dog1", "", 15, 25)]
    [InlineData("Dog1", "Brown", -1, 25)]
    [InlineData("Dog1", "Brown", 15, 0)]
    public async Task CreateDog_WithInvalidData_ReturnsBadRequest(
        string name, string color, int tailLength, int weight)
    {
        var request = new { name, color, tailLength, weight };

        // Act
        var response = await HttpClient.PostAsJsonAsync(BaseUrl, request, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateDog_WithDuplicateName_ReturnsConflict()
    {
        // Arrange
        var existingDog = DogSeedData.InitialDogs.First();
        var createDogRequest = new
        {
            name = existingDog.Name,
            color = "Brown",
            tail_length = 15,
            weight = 25
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDogRequest, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
    
    [Fact]
    public async Task CreateDog_WithValidDataButWithExtraProperty_ReturnsCreated()
    {
        var uniqueName = $"TestDog_{Guid.NewGuid()}";
        var createDogRequest = new
        {
            name = uniqueName,
            color = "Brown",
            tail_length = 15,
            weight = 25,
            extra_property = "Some value"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync(BaseUrl, createDogRequest, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
   
        var createdDog = await response.Content.ReadFromJsonAsync<DogDto>(JsonOptions);
        createdDog.Should().NotBeNull();
        createdDog!.Name.Should().Be(uniqueName);
    }
}