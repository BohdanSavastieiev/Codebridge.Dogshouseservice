using System.Net;
using System.Net.Http.Json;
using Codebridge.TechnicalTask.API.Common.Constants;
using Codebridge.TechnicalTask.Application.Dogs.Common;
using Codebridge.TechnicalTask.Domain.Dogs.Entities;
using Codebridge.TechnicalTask.Infrastructure.Persistence.Seed;
using Codebridge.TechnicalTask.IntegrationTests.Abstractions;
using FluentAssertions;

namespace Codebridge.TechnicalTask.IntegrationTests.Tests;

public class DogsEndpointsTests : BaseIntegrationTest
{
    private const string BaseUrl = $"{ApiConstants.ApiVersionedPath}/dogs";

    public DogsEndpointsTests(TestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetDog_WithValidName_ReturnsDog()
    {
        const string dogName = "Jessy";
        var url = $"{BaseUrl}/{dogName}";

        // Act
        var response = await HttpClient.GetAsync(url);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var dog = await response.Content.ReadFromJsonAsync<DogDto>(JsonOptions);
        dog.Should().NotBeNull();

        var expectedDog = DogSeedData.InitialDogs.First(d => d.Name == dogName);
        dog.Should().BeEquivalentTo(expectedDog, options => options
            .Including(d => d.Name)
            .Including(d => d.Color)
            .Including(d => d.TailLength)
            .Including(d => d.Weight));
    }

    [Fact]
    public async Task GetDogs_ReturnsSuccessStatusCodeWithCorrectDataAndHeaders()
    {
        // Arrange
        var pageSize = 2;
        var pageNumber = 3;
        var url = $"{BaseUrl}?pageSize={pageSize}&pageNumber={pageNumber}";
        var totalItems = DogSeedData.InitialDogs.Count;
        var orderedSeedDogs = DogSeedData.InitialDogs.OrderBy(d => d.Name).ToList();

        var expectedDogsOnPage = orderedSeedDogs
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Act
        var response = await HttpClient.GetAsync(url);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        VerifyPaginationHeaders(response, pageNumber, pageSize, totalItems);

        var dogs = await response.Content.ReadFromJsonAsync<List<DogDto>>(JsonOptions);
        dogs.Should().NotBeNull();
        dogs.Should().BeEquivalentTo(expectedDogsOnPage, options => options
            .Including(d => d.Name)
            .Including(d => d.Color)
            .Including(d => d.TailLength)
            .Including(d => d.Weight));
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(-1, 10)]
    [InlineData(1, -1)]
    public async Task GetDogs_WithInvalidPagination_ReturnsBadRequest(int pageNumber, int pageSize)
    {
        // Arrange
        var url = $"{BaseUrl}?pageSize={pageSize}&pageNumber={pageNumber}";

        // Act
        var response = await HttpClient.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData("invalid_attribute", "asc")]
    [InlineData("", "asc")]
    [InlineData(null, "desc")]
    [InlineData("name", "invalid_order")]
    public async Task GetDogs_WithInvalidSorting_ReturnsBadRequest(string attribute, string? order)
    {
        // Arrange
        var url = $"{BaseUrl}?attribute={attribute}&order={order}";

        // Act
        var response = await HttpClient.GetAsync(url);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("name", "asc")]
    [InlineData("tail_length", "DESC")]
    [InlineData("name", null)]
    [InlineData("tail_length", "")]
    public async Task GetDogs_WithSorting_ReturnsCorrectlyOrderedDogs(string attribute, string? order)
    {
        // Arrange
        var url = $"{BaseUrl}?attribute={attribute}&order={order ?? string.Empty}";

        // Act
        var response = await HttpClient.GetAsync(url);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var dogs = await response.Content.ReadFromJsonAsync<List<DogDto>>(JsonOptions);
        dogs.Should().NotBeNull();

        var orderedSeedDogs = order?.ToLower() == "desc"
            ? DogSeedData.InitialDogs.OrderByDescending(GetSortProperty(attribute))
            : DogSeedData.InitialDogs.OrderBy(GetSortProperty(attribute));

        dogs.Should().BeEquivalentTo(orderedSeedDogs, options => options
            .Including(d => d.Name)
            .Including(d => d.Color)
            .Including(d => d.TailLength)
            .Including(d => d.Weight));
    }

    [Theory]
    [InlineData("name", "desc", 2, 2)]
    [InlineData("weight", null, 3, 1)]
    [InlineData("name", "asc", 100, 1)]
    [InlineData("color", "asc", 2, 3)] 
    [InlineData("tail_length", "ASC", 3, 2)]
    public async Task GetDogs_WithSortingAndPagination_ReturnsCorrectlyOrderedAndPaginatedDogs(
        string attribute, string? order, int pageSize, int pageNumber)
    {
        // Arrange
        var url = $"{BaseUrl}?attribute={attribute}&order={order ?? string.Empty}&pageSize={pageSize}&pageNumber={pageNumber}";

        // Act
        var response = await HttpClient.GetAsync(url);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var dogs = await response.Content.ReadFromJsonAsync<List<DogDto>>(JsonOptions);
        dogs.Should().NotBeNull();

        var orderedSeedDogs = order?.ToLower() == "desc"
            ? DogSeedData.InitialDogs.OrderByDescending(GetSortProperty(attribute))
            : DogSeedData.InitialDogs.OrderBy(GetSortProperty(attribute));

        var expectedDogs = orderedSeedDogs
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        dogs.Should().BeEquivalentTo(expectedDogs, options => options
            .Including(d => d.Name)
            .Including(d => d.Color)
            .Including(d => d.TailLength)
            .Including(d => d.Weight));
    }

    private static Func<Dog, object> GetSortProperty(string attribute) => attribute.ToLower() switch
    {
        "name" => dog => dog.Name,
        "color" => dog => dog.Color,
        "tail_length" => dog => dog.TailLength,
        "weight" => dog => dog.Weight,
        _ => throw new ArgumentException($"Invalid sort attribute: {attribute}")
    };

    private static void VerifyPaginationHeaders(HttpResponseMessage response, int pageNumber, int pageSize,
        int totalItems)
    {
        var expectedTotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var headers = response.Headers;
        headers.Should().ContainKey(ApiConstants.HttpHeaders.PageNumber)
            .WhoseValue.Should().ContainSingle(pageNumber.ToString());

        headers.Should().ContainKey(ApiConstants.HttpHeaders.PageSize)
            .WhoseValue.Should().ContainSingle(pageSize.ToString());

        headers.Should().ContainKey(ApiConstants.HttpHeaders.TotalPages)
            .WhoseValue.Should().ContainSingle(expectedTotalPages.ToString());

        headers.Should().ContainKey(ApiConstants.HttpHeaders.TotalCount)
            .WhoseValue.Should().ContainSingle(totalItems.ToString());
    }
}